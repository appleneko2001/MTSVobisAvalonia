using MTSVobisAvalonia.Classes;
using MTSVobisAvalonia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTSVobisAvalonia
{
    public class ModemService
    {
        public static ModemService Instance => _instance ?? throw new NullReferenceException("Not ready yet!!!");

        public static void Init(string ip)
        {
            _instance ??= new ModemService(ip);
        }
        
        public string IpModem => m_IpModem;
        
        /// <summary>
        /// When modem disconnected or cannot reached to.
        /// </summary>
        public event EventHandler? ModemDisconnected;

        public event EventHandler? ModemConnected;

        public event EventHandler<SmsTotalMessagesModel>? SmsInboxUpdated;

        public event EventHandler<ModemStatusModel>? ModemStatusUpdated;

        public event EventHandler<SmsCapacityInfoModel>? MessagesStatusUpdated;

        //private Timer m_PeriodicalGetStatusTimer;
        private static ModemService? _instance;
        private readonly string m_IpModem;
        private bool m_LastTimeWasOffline;
        
        private ModemService(string ip)
        {
            m_IpModem = ip;
            _instance = this;

            Task.Factory.StartNew(BackgroundServiceCallback);
        }

        private void BackgroundServiceCallback()
        {
            // TODO: Break loop if the application closed
            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    var ping = new Ping();
                    var t = ping.SendPingAsync(m_IpModem).Result;

                    if (t.Status != IPStatus.Success)
                        throw new Exception("Timed out");
                
                    var status = TryGetResultFromModemJsonApi<ModemStatusModel>(ZteApi.GetPeriodicalDeviceStatus,
                        Array.Empty<string>());
                    
                    if (status != null)
                        UpdateStatus(status);

                    if (m_LastTimeWasOffline)
                    {
                        m_LastTimeWasOffline = false;
                        ModemConnected?.Invoke(this, EventArgs.Empty);
                    }

                    var smsResult = GetMessagesStatus();

                    if (smsResult != null)
                        MessagesStatusUpdated?.Invoke(this, smsResult);
                }
                catch
                {
                    ModemDisconnected?.Invoke (this, EventArgs.Empty);
                    m_LastTimeWasOffline = true;
                } 
            }
        }

        private void UpdateStatus (ModemStatusModel status)
        {
            ModemStatusUpdated?.Invoke(this, status);
            if (status.SmsReceivedFlag == 0)
                return;
            
            var args = GetAllReceivedMessages();
            
            if(args != null)
                SmsInboxUpdated?.Invoke(this, args);
        }

        public SmsCapacityInfoModel? GetMessagesStatus()
        {
            var result = Utils.GetResponse($"{IpModem}{ZteApi.GetSmsCapacityInfo}");
            return JsonConvert.DeserializeObject<SmsCapacityInfoModel>(result);
        }
        
        /// <summary>
        /// Try get all received messages from modem storage. Limit messages of a single request is 500.
        /// </summary>
        public SmsTotalMessagesModel? GetAllReceivedMessages()
        {
            return TryGetResultFromModemJsonApi<SmsTotalMessagesModel>(ZteApi.GetSmsData, new[]
            {
                "page=0",
                "data_per_page=500",
                "mem_store=1",
                "tags=10",
                "order_by=order+by+id+desc"
            });
        }

        public async Task<SmsTotalMessagesModel?> GetAllReceivedMessagesAsync()
        {
            return await TryGetResultFromModemJsonApiAsync<SmsTotalMessagesModel>(ZteApi.GetSmsData, new[]
            {
                "page=0",
                "data_per_page=500",
                "mem_store=1",
                "tags=10",
                "order_by=order+by+id+desc"
            });
        }

        /// <summary>
        /// Make selected messages as read and seen content already. Can be one or more msg to be marked.
        /// </summary>
        public bool SetSmsRead(string[] ids)
        {
            return TryGetResultFromModemJsonApi<ResultModel>(ZteApi.SetSmsRead, SetSmsReadArgs(ids))?
                .IsSuccess ?? false;
        }

        public async Task<bool> SetSmsReadAsync(string[] ids)
        {
            var r = await TryGetResultFromModemJsonApiAsync<ResultModel>(ZteApi.SetSmsRead, SetSmsReadArgs(ids));
            return r?.IsSuccess ?? false;
        }
        
        /// <summary>
        /// Delete selected messages (can be one or more, depends how much you are actually selected to delete)
        /// </summary>
        public bool DeleteMessages(IList<SmsDataItemModel> selected)
        {
            return TryGetResultFromModemJsonApi<ResultModel>(ZteApi.SetDeleteSms, DeleteMessagesArgs(selected))?
                .IsSuccess ?? false;
        }

        public async Task<bool> DeleteMessagesAsync(IList<SmsDataItemModel> selected)
        {
            var r = await TryGetResultFromModemJsonApiAsync<ResultModel>(ZteApi.SetDeleteSms,
                DeleteMessagesArgs(selected));
            return r?.IsSuccess ?? false;
        }

        private string[] SetSmsReadArgs(string[] ids)
        {
            var builder = new StringBuilder("&msg_id=");
            foreach (var id in ids) builder.Append($"{id};");
            return new[]
            {
                builder.ToString(),
                "tag=0"
            };
        }
        
        private string[] DeleteMessagesArgs(IList<SmsDataItemModel> selected)
        {
            var builder = new StringBuilder("&msg_id=");
            foreach (var sms in selected) builder.Append($"{sms.Id};");
            return new[]
            {
                builder.ToString(),
                "notCallback=true"
            }; 
        }

        public T? TryGetResultFromModemJsonApi<T>(string api, string[] args)
        {
            var result = Utils.GetResponse(IpModem + api, parameters: string.Join('&', args));
            return JsonConvert.DeserializeObject<T>(result);
        }
        
        public async Task<T?> TryGetResultFromModemJsonApiAsync<T>(string api, string[] args)
        {
            var result = await Utils.GetResponseAsync(IpModem + api, parameters: string.Join('&', args));
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(result));
        }
    }
}
