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

            var warningInformation = new WarningInformation();
            warningInformation.SLStations = stations;
            warningInformation.AppendWarningInformation(statusMessage);

            return statusMessage;
        }
    }
}
