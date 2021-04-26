using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTSVobisAvalonia.Models
{
    public class SmsCapacityInfoModel
    {
        [JsonProperty("sms_nv_total")]
        public string SmsCapacityTotal;
        [JsonProperty("sms_sim_total")]
        public string SmsSimCapacityTotal;

        [JsonProperty("sms_nv_rev_total")]
        public string SmsReceivedTotal;
        [JsonProperty("sms_nv_send_total")]
        public string SmsSentTotal;
        [JsonProperty("sms_nv_draftbox_total")]
        public string SmsDraftboxTotal;

        [JsonProperty("sms_sim_rev_total")]
        public string SmsSimReceivedTotal;
        [JsonProperty("sms_sim_send_total")]
        public string SmsSimSentTotal;
        [JsonProperty("sms_sim_draftbox_total")]
        public string SmsSimDraftboxTotal;
    }
}
