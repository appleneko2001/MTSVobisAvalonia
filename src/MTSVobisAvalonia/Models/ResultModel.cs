using Newtonsoft.Json;

namespace MTSVobisAvalonia.Models
{
    public class ResultModel
    { 
        [JsonProperty("result")]
        public string Result = string.Empty;
        public bool IsSuccess => Result == "success";
    }
}
