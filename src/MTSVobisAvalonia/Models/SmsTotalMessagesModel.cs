using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MTSVobisAvalonia.Models
{
    public class SmsTotalMessagesModel
    {
        [JsonProperty("messages")]
        public SmsDataItemModel[] Messages;
    }
}
