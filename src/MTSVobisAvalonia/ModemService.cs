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
        /// <summary>
        /// When modem disconnected or cannot reached to.
        /// </summary>
        public event EventHandler ModemDisconnected;

        public event EventHandler<SmsTotalMessagesModel> SmsInboxUpdated;

        public event EventHandler<ModemStatusModel> ModemStatusUpdated;

        public event EventHandler<SmsCapacityInfoModel> MessagesStatusUpdated;

        public static ModemService Instance { get; private set; }
        public string IPModem { get; private set; }

        private Timer m_PeriodicalGetStatusTimer;
        public ModemService(string ip)
        {
            IPModem = ip;
            m_PeriodicalGetStatusTimer = new Timer(1000);
            m_PeriodicalGetStatusTimer.Elapsed += GetStatusTimer_DoWork;
            m_PeriodicalGetStatusTimer.Start();
            Instance = this;
        }

        private void GetStatusTimer_DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                var result = Utils.GetResponse(IPModem + ZTEApi.GET_PERIODICAL_DEVICE_STATUS);
                var status = JsonConvert.DeserializeObject<ModemStatusModel>(result);

                var smsResult = GetMessagesStatus();
                MessagesStatusUpdated?.Invoke(this, smsResult);
                UpdateStatus(status);
            }
            catch
            {
                ModemDisconnected?.Invoke (this, null);
            } 
        }
        private void UpdateStatus (ModemStatusModel status)
        {
            ModemStatusUpdated?.Invoke(this, status);
            if (status.SmsReceivedFlag != 0)
            {
                var args = GetAllMessages();
                SmsInboxUpdated?.Invoke(this, args);
            }
        }

        public SmsCapacityInfoModel GetMessagesStatus()
        {
            var result = Utils.GetResponse(IPModem + ZTEApi.GET_SMS_CAPACITY_INFO);
            return JsonConvert.DeserializeObject<SmsCapacityInfoModel>(result);
        }
        public SmsTotalMessagesModel GetAllMessages()
        {
            var builder = new StringBuilder();
            builder.Append("&page=0");
            builder.Append("&data_per_page=500");
            builder.Append("&mem_store=1");
            builder.Append("&tags=10");
            builder.Append("&order_by=order+by+id+desc");
            var result = Utils.GetResponse(IPModem + ZTEApi.GET_SMS_DATA, parameters: builder.ToString());
            return JsonConvert.DeserializeObject<SmsTotalMessagesModel>(result);
        }
        public bool SetSmsReaded(string id)
        {
            var builder = new StringBuilder();
            builder.Append($"&msg_id={id};");
            builder.Append($"&tag=0");
            var result = Utils.GetResponse(IPModem + ZTEApi.SET_SMS_READED, parameters: builder.ToString());
            return JsonConvert.DeserializeObject<ResultModel>(result).IsSuccess;
        } 
        public bool DeleteMessages(IEnumerable<SmsDataItemModel> selected)
        {
            var builder = new StringBuilder();
            builder.Append("&msg_id=");
            foreach (var sms in selected)
            {
                builder.Append($"{sms.Id};");
            }
            builder.Append("&notCallback=true");
            var result = Utils.GetResponse(IPModem + ZTEApi.SET_DELETE_SMS, parameters: builder.ToString());
            return JsonConvert.DeserializeObject<ResultModel>(result).IsSuccess;
        }
    }
}
