using RoundAboutNow.Api.Messages;
using RoundAboutNow.Api.Models.Api.SL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models
{
    public static class StatusMessageCreator
    {
        public static async Task<StatusMessage> GetGeneralStatusMessageAsync(string latitude, string longitude)
        {
            var statusMessage = new StatusMessage();

            //TODO: Hämta location från separat API
            statusMessage.Location = "Stockholm";

            var stationsCloseBy = new SLStationsCloseByApi(KeyKeeper.GetSLCloseByStationsKey(), latitude, longitude);
            var stations = await stationsCloseBy.GetSLStations();

            if (stations.Count > 0)
            {
                string siteIDs = "";

                for (int i = 0; i < stations.Count; i++)
                {
                    siteIDs += stations[i].Id.ToString().Substring(stations[i].Id.ToString().Length - 4, 4);
                    if (i != stations.Count)
                        siteIDs += ",";
                }

                var disturbances = new SLDisturbancesApi(KeyKeeper.GetSLDisturbanceKey());
                disturbances.SiteID = siteIDs;

                var slDisturbances = await disturbances.GetSLDisturbances();

                //Testingpurposes
                statusMessage.WarningMessage = slDisturbances.Count().ToString();
            }

            // TODO: Disturbances by stations
            return statusMessage;
        }
    }
}
