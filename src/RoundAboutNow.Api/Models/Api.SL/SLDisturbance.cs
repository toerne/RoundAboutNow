using Newtonsoft.Json;

namespace RoundAboutNow.Api.Models.Api.SL
{
    public class SLDisturbance
    {
        public string Created { get; set; }
        public string Header { get; set; }
        public string Details { get; set; }
        public string ScopeElements { get; set; }
        [JsonProperty("FromDateTime")]
        public string StartDate { get; set; }
        [JsonProperty("UpToDateTime")]
        public string EndDate { get; set; }
        public string Updated { get; set; }
    }
}