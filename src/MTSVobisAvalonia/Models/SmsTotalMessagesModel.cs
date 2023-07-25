using Newtonsoft.Json;

namespace MTSVobisAvalonia.Models
{
    public class SmsTotalMessagesModel
    {
        [JsonProperty("messages")]
        public SmsDataItemModel[]? Messages;
    }
}
