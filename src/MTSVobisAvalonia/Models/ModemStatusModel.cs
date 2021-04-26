using Newtonsoft.Json; 

namespace MTSVobisAvalonia.Models
{
    public class ModemStatusModel
    {
        [JsonProperty("loginfo")]
        public string Status;
        [JsonProperty("network_type")]
        public string NetworkType;
        [JsonProperty("sub_network_type")]
        public string SubNetworkType;

        [JsonProperty("realtime_tx_bytes")]
        public string RealtimeTransmitedBytesString;
        [JsonProperty("realtime_rx_bytes")]
        public string RealtimeReceivedBytesString;
        [JsonProperty("realtime_time")]
        public string RealtimeConnectedTimeString;
        public ulong RealtimeTransmitedBytes => RealtimeTransmitedBytesString.ParseUInt64(); 
        public ulong RealtimeReceivedBytes => RealtimeReceivedBytesString.ParseUInt64(); 
        public ulong RealtimeConnectedTime => RealtimeConnectedTimeString.ParseUInt64(); 

        [JsonProperty("monthly_tx_bytes")]
        public string MonthlyTransmitedBytesString;
        [JsonProperty("monthly_rx_bytes")]
        public string MonthlyReceivedBytesString;
        public ulong MonthlyTransmitedBytes => MonthlyTransmitedBytesString.ParseUInt64();
        public ulong MonthlyReceivedBytes => MonthlyReceivedBytesString.ParseUInt64(); 

        [JsonProperty("network_provider")]
        public string MobileProvider;
        [JsonProperty("signalbar")]
        public string SignalBarString;
         
        [JsonProperty("rssi")]
        public string RSSIString;
        public int RSSI => RSSIString.ParseInt32();
        [JsonProperty("sim_imsi")]
        public string SimImsiString;
        [JsonProperty("imei")]
        public string ImeiString;

        [JsonProperty("wan_ipaddr")]
        public string WanAddressString;
        [JsonProperty("ipv6_wan_ipaddr")]
        public string WanAddress6String;

        [JsonProperty("sms_received_flag")]
        public string SmsReceivedFlagString;
        public int SmsReceivedFlag => SmsReceivedFlagString.ParseInt32();
        [JsonProperty("sms_unread_num")]
        public string SmsUnreadNumString;
        public int SmsUnreadNum => SmsUnreadNumString.ParseInt32();
    }
}
