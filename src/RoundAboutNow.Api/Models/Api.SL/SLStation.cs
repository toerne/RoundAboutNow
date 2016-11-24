using Newtonsoft.Json;

namespace RoundAboutNow.Api.Models.Api.SL
{
    public class SLStation
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("lat")]
        public string Latitude { get; set; }

        [JsonProperty("lon")]
        public string Longitude { get; set; }

        [JsonProperty("dist")]
        public int Distance { get; set; }
    }
}