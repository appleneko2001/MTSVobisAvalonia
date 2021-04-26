using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTSVobisAvalonia.Models
{
    public class SmsDataItemModel : ViewModelBase
    {
        [JsonProperty("id")]
        public string Id { get; private set; }
        [JsonProperty("number")]
        public string From { get; private set; }
        private string m_Content;
        [JsonProperty("content")]
        public string Content 
        { 
            get => m_Content; 
            private set { m_Content = value.UnicodeDecode(); } 
        }
        [JsonProperty("date")]
        public string DateString { get; private set; }

        private string m_TagString;
        [JsonProperty("tag")]
        public string TagString 
        { 
            get => m_TagString;
            private set { 
                m_TagString = value; 
                this.RaisePropertyChanged(); 
            }
        }

        public string GetClasses => TagString == "1" ? "Unread" : "";
        public bool IsUnread => TagString == "1";

        public void SetRead(bool isRead) => TagString = "0";

        public string ContentPreview => m_Content.Trimming(40);


        public override string ToString()
        {
            return $"{From}: {ContentPreview}";
        }
    }
}
