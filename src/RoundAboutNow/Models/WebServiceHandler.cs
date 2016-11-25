using Newtonsoft.Json;
using RoundAboutNow.Api.Messages;
using RoundAboutNow.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoundAboutNow.Models
{
    public static class WebServiceHandler
    {
        public static string ws = "http://localhost:32394/api/status/";
        static async public Task<LocationStatusVM> GetLocationGeneralStatusMessageAsync(Coordinate coordinate)
        {
            var url = $"{ws}{coordinate.Latitude},{coordinate.Longitude}";
            var client = new HttpClient();

            var json = await client.GetStringAsync(url);
            var statusMessage = JsonConvert.DeserializeObject<StatusMessage>(json);

            var viewModel = new LocationStatusVM
            {
                WarningMessage = statusMessage.WarningMessage,
                Header = statusMessage.Location,
                WarningLevel = statusMessage.WarningLevel
            };

            return viewModel;
        }
    }
}
