using Newtonsoft.Json;

namespace MTSVobisAvalonia.Models
{
    public class SmsCapacityInfoModel
    {
        [JsonProperty("sms_nv_total")]
        public string SmsCapacityTotal = string.Empty;
        [JsonProperty("sms_sim_total")]
        public string SmsSimCapacityTotal = string.Empty;

        [JsonProperty("sms_nv_rev_total")]
        public string SmsReceivedTotal = string.Empty;
        [JsonProperty("sms_nv_send_total")]
        public string SmsSentTotal = string.Empty;
        [JsonProperty("sms_nv_draftbox_total")]
        public string SmsDraftBoxTotal = string.Empty;

        [JsonProperty("sms_sim_rev_total")]
        public string SmsSimReceivedTotal = string.Empty;
        [JsonProperty("sms_sim_send_total")]
        public string SmsSimSentTotal = string.Empty;
        [JsonProperty("sms_sim_draftbox_total")]
        public string SmsSimDraftBoxTotal = string.Empty;
    }
}
