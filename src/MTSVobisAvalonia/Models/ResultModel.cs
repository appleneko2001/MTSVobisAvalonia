using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTSVobisAvalonia.Models
{
    public class ResultModel
    { 
        [JsonProperty("result")]
        public string Result;
        public bool IsSuccess => Result == "success";
    }
}
