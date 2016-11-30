using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoundAboutNow.Models;
using System.Net.Http;
using RoundAboutNow.Models.ViewModels;
using Newtonsoft.Json;
using RoundAboutNow.Api.Messages;

namespace RoundAboutNow.Controllers
{
    public class StatusController : Controller
    {
        public IActionResult CurrentStatus()
        {
            return View();
        }


        public async Task<PartialViewResult> LocationStatus(Coordinate coordinate)
        {
            try
            {
                var viewModel = await WebServiceHandler.GetLocationGeneralStatusMessageAsync(coordinate);
                return PartialView("_LocationStatus", viewModel);

            }
            catch (Exception ex)
            {
                var viewModel = new LocationStatusVM { WarningLevel = -1, DisturbanceWarningMessage = ex.Message, Header = "*Error*", WeatherWarningMessage = "Error weather!"};
                return PartialView("_LocationStatus", viewModel);
            }
        }
    }
}
