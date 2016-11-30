using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public class SingleWarning
    {
        [JsonProperty("info")]
        public Info info { get; set; }        
    }
}
