using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SL
{
    public class SLDisturbancesApi
    {
        public string TransportMode { get; set; }
        public string LineNumber { get; set; }
        public string SiteID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Key { get; set; }

        public SLDisturbancesApi(string key)
        {
            Key = key;
        }

        private string CreateUrl()
        {

            //TODO varför skiljer sig mängd för rawdata kontra ej raw data?
            var result = $"http://api.sl.se/api2/deviations.json?key={Key}";

            if (!String.IsNullOrEmpty(TransportMode))
            {
                result += $"&transportMode={TransportMode}";
            }
            if (!String.IsNullOrEmpty(LineNumber))
            {
                result += $"&lineNumber={LineNumber}";
            }
            if (!String.IsNullOrEmpty(SiteID))
            {
                result += $"&siteId={SiteID}";
            }
            if (!String.IsNullOrEmpty(FromDate) && !String.IsNullOrEmpty(ToDate))
            {
                result += $"&fromDate={FromDate}&toDate{ToDate}";
            }

            return result;    
        }

        public async Task<List<SLDisturbance>> GetSLDisturbances()
        {
            var url = CreateUrl();
            var handler = new WebServiceHandler();

            string json = await handler.GetResultFromAPIAsync(url);

            var wrapper = new
            {
                ResponseData = new List <SLDisturbance>()

            };

            var result = JsonConvert.DeserializeAnonymousType(json, wrapper);
            return result.ResponseData;
        }
    }
}
