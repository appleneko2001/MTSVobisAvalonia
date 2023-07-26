using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MTSVobisAvalonia
{
    public static class Utils
    {
        public const string DEFAULT_USERAGENT = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:84.0) Gecko/20100101 Firefox/84.0";
        
        public static string AppendTimestamp(string url) => $"{url}&_={new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()}";

        public static async Task<string> GetResponseAsync(string url, string method = "GET", string parameters = "", bool appendTimestamp=true)
        {
            // Although the ZTE (or MTS 8810FT) are using HTTP without certs to access management UI.
            // the rarely possibility would appear that some modem are using HTTPS instead.
            // Please tell me if your Modem WebUI does using HTTPS instead of HTTP. 
            if (!url.ToLower().StartsWith("http://"))
            {
                url = "http://" + url;
            }

            if (!parameters.StartsWith('&'))
                parameters = parameters.Insert(0, "&");

            var urlMixed = $"{url}{(string.IsNullOrWhiteSpace(parameters) ? "" : parameters)}";
            if (appendTimestamp)
                urlMixed = AppendTimestamp(urlMixed);
            var request = (HttpWebRequest)WebRequest.Create(urlMixed);
            //request.UserAgent = DEFAULT_USERAGENT;
            request.Method = method;
            var responseTask = Task.Factory.FromAsync
                                      (request.BeginGetResponse,
                                       request.EndGetResponse,
                                       null);
            using var response = (HttpWebResponse)await responseTask;
            await using var origin = response.GetResponseStream();
            using var reader = new StreamReader(origin);
            return await reader.ReadToEndAsync();
        }

        public static string GetResponse(string url, string method = "GET", string parameters = "", bool appendTimestamp=true)
        {
            if (!url.ToLower().StartsWith("http://"))
            {
                url = "http://" + url;
            }

            if (!parameters.StartsWith('&'))
                parameters = parameters.Insert(0, "&");

            var urlMixed = $"{url}{(string.IsNullOrWhiteSpace(parameters) ? "" : parameters)}";
            if (appendTimestamp)
                urlMixed = AppendTimestamp(urlMixed);

            var request = (HttpWebRequest)WebRequest.Create(urlMixed);
            //request.UserAgent = DEFAULT_USERAGENT;
            request.Method = method;
            using var response = (HttpWebResponse)request.GetResponse();
            using var origin = response.GetResponseStream();
            using var reader = new StreamReader(origin);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Decode numerical unicode text as regular utf8 text.
        /// </summary>
        public static string UnicodeDecode (this string text)
        { 
            var fullString = "";
            var block = ""; 
            foreach(var c in text)
            { 
                block += c;

                if (block.Length >= 4)
                {
                    fullString += (char)Convert.ToInt32(block, 16);
                    block = "";
                }
            }
            return fullString;
        }

        public static string Trimming(this string text, int length = 10) 
        {
            var len = text.Length;
            if (len < length)
                return text;
            var processed = text.Replace("\r", "").Replace("\n", "").Substring(0, length);
            return processed + (len >= length + 2 ? "..." : "");
        }

        /// <summary>
        /// Parse time string as DateTime instance.
        /// </summary>
        public static DateTime TransTime(this string str)
        {
            var parts = str.Split(',');
            return new DateTime(
                int.Parse(parts[0]), 
                int.Parse(parts[1]), 
                int.Parse(parts[2]), 
                int.Parse(parts[3]), 
                int.Parse(parts[4]), 
                int.Parse(parts[5]))
                .AddYears(2000); // Fix 00xx year display error
            // ZTE Vobis firmware sucks
        }

        public static ulong ParseUInt64(this string str) => ulong.TryParse(str, out var r) ? r : 0;
        public static int ParseInt32(this string str) => int.TryParse(str, out var r) ? r : 0;

        public static string? ReadAllText(this string path) => File.Exists(path) ? File.ReadAllText(path) : null;
    }
}
