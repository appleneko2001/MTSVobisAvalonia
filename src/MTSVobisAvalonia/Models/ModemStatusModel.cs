using Newtonsoft.Json; 

namespace MTSVobisAvalonia.Models
{
    public class ModemStatusModel
    {
        [JsonProperty("loginfo")]
        public string Status = string.Empty;
        [JsonProperty("network_type")]
        public string NetworkType = string.Empty;
        [JsonProperty("sub_network_type")]
        public string SubNetworkType = string.Empty;

        [JsonProperty("realtime_tx_bytes")]
        public string RealtimeTransmitedBytesString = string.Empty;
        [JsonProperty("realtime_rx_bytes")]
        public string RealtimeReceivedBytesString = string.Empty;
        [JsonProperty("realtime_time")]
        public string RealtimeConnectedTimeString = string.Empty;
        public ulong RealtimeTransmittedBytes => RealtimeTransmitedBytesString.ParseUInt64(); 
        public ulong RealtimeReceivedBytes => RealtimeReceivedBytesString.ParseUInt64(); 
        public ulong RealtimeConnectedTime => RealtimeConnectedTimeString.ParseUInt64(); 

        [JsonProperty("monthly_tx_bytes")]
        public string MonthlyTransmittedBytesString = string.Empty;
        [JsonProperty("monthly_rx_bytes")]
        public string MonthlyReceivedBytesString = string.Empty;
        public ulong MonthlyTransmittedBytes => MonthlyTransmittedBytesString.ParseUInt64();
        public ulong MonthlyReceivedBytes => MonthlyReceivedBytesString.ParseUInt64(); 

        [JsonProperty("network_provider")]
        public string MobileProvider = string.Empty;
        [JsonProperty("signalbar")]
        public string SignalBarString = string.Empty;
         
        [JsonProperty("rssi")]
        public string RssiString = string.Empty;
        public int Rssi => RssiString.ParseInt32();
        [JsonProperty("sim_imsi")]
        public string SimImsiString = string.Empty;
        [JsonProperty("imei")]
        public string ImeiString = string.Empty;

        [JsonProperty("wan_ipaddr")]
        public string WanAddressString = string.Empty;
        [JsonProperty("ipv6_wan_ipaddr")]
        public string WanAddress6String = string.Empty;

        [JsonProperty("sms_received_flag")]
        public string SmsReceivedFlagString = string.Empty;
        public int SmsReceivedFlag => SmsReceivedFlagString.ParseInt32();
        [JsonProperty("sms_unread_num")]
        public string SmsUnreadNumString = string.Empty;
        public int SmsUnreadNum => SmsUnreadNumString.ParseInt32();
    }
}
