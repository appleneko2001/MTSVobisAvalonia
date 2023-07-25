using MTSVobisAvalonia.Classes;
using MTSVobisAvalonia.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

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

        public event EventHandler<SmsTotalMessagesModel>? SmsInboxUpdated;

        public event EventHandler<ModemStatusModel>? ModemStatusUpdated;

        public event EventHandler<SmsCapacityInfoModel>? MessagesStatusUpdated;

        private Timer m_PeriodicalGetStatusTimer;
        private static ModemService? _instance;
        private readonly string m_IpModem;
        
        private ModemService(string ip)
        {
            m_IpModem = ip;
            m_PeriodicalGetStatusTimer = new Timer(1000);
            m_PeriodicalGetStatusTimer.Elapsed += GetStatusTimer_DoWork;
            m_PeriodicalGetStatusTimer.Start();
            _instance = this;
        }

        private void GetStatusTimer_DoWork(object? sender, ElapsedEventArgs e)
        {
            try
            {
                var status =
                    TryGetResultFromModemJsonApi<ModemStatusModel>(ZTEApi.GET_PERIODICAL_DEVICE_STATUS,
                        Array.Empty<string>());

                var smsResult = GetMessagesStatus();
                
                if(smsResult != null)
                    MessagesStatusUpdated?.Invoke(this, smsResult);
                
                if(status != null)
                    UpdateStatus(status);
            }
            catch
            {
                ModemDisconnected?.Invoke (this, EventArgs.Empty);
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
            var result = Utils.GetResponse($"{IpModem}{ZTEApi.GET_SMS_CAPACITY_INFO}");
            return JsonConvert.DeserializeObject<SmsCapacityInfoModel>(result);
        }
        
        /// <summary>
        /// Try get all received messages from modem storage. Limit messages of a single request is 500.
        /// </summary>
        public SmsTotalMessagesModel? GetAllReceivedMessages()
        {
            return TryGetResultFromModemJsonApi<SmsTotalMessagesModel>(ZTEApi.GET_SMS_DATA, new[]
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
            var builder = new StringBuilder("&msg_id=");
            foreach (var id in ids) builder.Append($"{id};");

            return TryGetResultFromModemJsonApi<ResultModel>(ZTEApi.SET_SMS_READED, new[]
            {
                builder.ToString(),
                "tag=0"
            })?.IsSuccess ?? false;
        } 
        
        /// <summary>
        /// Delete selected messages (can be one or more, depends how much you are actually selected to delete)
        /// </summary>
        public bool DeleteMessages(IList<SmsDataItemModel> selected)
        {
            var builder = new StringBuilder("&msg_id=");
            foreach (var sms in selected) builder.Append($"{sms.Id};");
            
            return TryGetResultFromModemJsonApi<ResultModel>(ZTEApi.SET_DELETE_SMS, new[]
            {
                builder.ToString(),
                "notCallback=true"
            })?.IsSuccess ?? false;
        }

        public T? TryGetResultFromModemJsonApi<T>(string api, string[] args)
        {
            var result = Utils.GetResponse(IpModem + api, parameters: string.Join('&', args));
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}
