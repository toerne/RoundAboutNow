using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SL
{
    public class SLStationsCloseByApi
    {
        //API Nyckel
        public string Key { get; set; }
        //Latitud
        public string OriginCoordLat { get; set; }
        //Longitud
        public string OriginCoordLong { get; set; }
        //MaxResults
        public int MaxResults { get; set; } = 1000;
        //Radie i meter
        public int Radius { get; set; }
        //Språk sv/en, default sv
        public string Lang { get; set; }

        public SLStationsCloseByApi(string key, string originCoordLat, string originCoordLong, int radius = 500)
        {
            Key = key;
            OriginCoordLat = originCoordLat;
            OriginCoordLong = originCoordLong;
            Radius = radius;
        }

        private string CreateUrl()
        {
            var result = $"http://api.sl.se/api2/nearbystops.json?key={Key}&originCoordLat={OriginCoordLat}&originCoordLong={OriginCoordLong}";

            if (MaxResults >= 1 && MaxResults <= 1000)
            {
                result += $"&maxresults={MaxResults}";
            }
            if (Radius >= 1 && Radius <= 2000)
            {
                result += $"&radius={Radius}";
            }
            if (Lang == "sv" || Lang == "en")
            {
                result += $"&lang={Lang}";
            }

            return result;
        }

        public async Task<List<SLStation>> GetSLStations()
        {            
            var url = CreateUrl();
            var handler = new WebServiceHandler();
            string json = await handler.GetResultFromAPIAsync(url);

            var wrapper = new
            {
                LocationList = new
                {
                    StopLocation = new List<SLStation>()
                }
            };

            var result = JsonConvert.DeserializeAnonymousType(json, wrapper);
            return result.LocationList.StopLocation;
             
        }
    }
}
