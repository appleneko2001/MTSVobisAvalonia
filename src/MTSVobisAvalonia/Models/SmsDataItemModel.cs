using Newtonsoft.Json;

namespace MTSVobisAvalonia.Models
{
    public class SmsDataItemModel
    {
        [JsonProperty("id")] public string Id { get; set; } = string.Empty;
        
        [JsonProperty("number")] public string From { get; set; } = string.Empty;

        [JsonProperty("content")]
        public string Content
        {
            get => m_Content; 
            set => m_Content = value.UnicodeDecode();
        }
        
        //get => m_Content; 
        //private set { m_Content = value.UnicodeDecode(); } 
        [JsonProperty("date")] public string DateString { get; private set; } = string.Empty;

        [JsonProperty("tag")] public string TagString { get; set; } = string.Empty;
        
        public string ContentPreview => Content.Trimming(30);

        private string m_Content = string.Empty;

        public override string ToString()
        {
            return $"{From}: {ContentPreview}";
        }
    }
}
