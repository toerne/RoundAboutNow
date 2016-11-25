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
            statusMessage.WarningMessage = "";

            var stationsCloseBy = new SLStationsCloseByApi(KeyKeeper.GetSLCloseByStationsKey(), latitude, longitude);
            var stations = await stationsCloseBy.GetSLStations();

            List<Task> taskList = new List<Task>();

            foreach (var station in stations)
            {
                try
                {
                    var newtask = new Task(() =>
                    {
                        var disturbancesApi = new SLDisturbancesApi(KeyKeeper.GetSLDisturbanceKey());
                        disturbancesApi.SiteID = station.Id.ToString().Substring(station.Id.ToString().Length - 4, 4);
                        var disturbancesTask = disturbancesApi.GetSLDisturbances();
                        station.Disturbances = disturbancesTask.Result;
                    });

                    newtask.Start();
                    taskList.Add(newtask);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            Task.WaitAll(taskList.ToArray());

            double stationsWithDisturbances = stations.Where(s => s.Disturbances.Count > 0).Count();
            double percentage = stationsWithDisturbances / stations.Count();
            statusMessage.WarningMessage = "Percentage: " + percentage + "%. Stationer med problem: " + stationsWithDisturbances + ". Totalt antal stationer: " + stations.Count();


            /*
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
            }*/

            // TODO: Disturbances by stations
            return statusMessage;
        }
    }
}
