using RoundAboutNow.Api.Messages;
using RoundAboutNow.Api.Models.Api.SL;
using RoundAboutNow.Api.Models.Api.SMHI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models
{
    public static class StatusMessageCreator
    {
        private static WarningInformation warningInformation;
        


        public static async Task<StatusMessage> GetGeneralStatusMessageAsync(string latitude, string longitude)
        {
            StatusMessage statusMessage = new StatusMessage();
            warningInformation = new WarningInformation();


            //TODO: Hämta location från separat API
            statusMessage.WarningMessage = "";
            warningInformation = await AddWeatherWarningStatusMessageAsync(latitude, longitude, warningInformation);
            warningInformation = await AddDisturbanceStatusMessageAsync(latitude, longitude, warningInformation);

            warningInformation.AppendWarningInformation(statusMessage);

            return statusMessage;
        }

        private async static Task<WarningInformation> AddWeatherWarningStatusMessageAsync(string latitude, string longitude, WarningInformation warningInfo)
        {
            try
            {
                var smhiWarningsApi = new SMHIWarningsApi();

                // Get district number by comparison to JSON-district answer from SMHI, apply that cool algorithm we thought of.
                // Check in JSON-alert answer from SMHI if there is a warning for this district

                SMHIWarning smhiWarning = await smhiWarningsApi.GetWarningsAsync(latitude, longitude);

                // Change warning-Level in Status-Message, Append WarningMessage
                warningInfo.SmhiWarning = smhiWarning;

                return warningInfo;
            }
            catch (Exception ex)
            {
                var exception = ex.Message;
            }

            return warningInfo;
        }

        private static async Task<WarningInformation> AddDisturbanceStatusMessageAsync(string latitude, string longitude, WarningInformation warningInfo)
        {
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

            
            warningInfo.SLStations = stations;

            return warningInfo;
        }
    }
}
