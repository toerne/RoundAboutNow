using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public class Info
    {
        [JsonProperty("severity")]
        public string Severity { get; set; }
        [JsonProperty("eventCode")]
        public List<Dictionary<string,string>> EventCode { get; set; }
        [JsonProperty("area")]
        public Dictionary<string, string> Area { get; set; }
    }
}
