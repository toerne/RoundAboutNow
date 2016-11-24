using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RoundAboutNow.Api.Messages;
using RoundAboutNow.Api.Models.Api.SL;
using RoundAboutNow.Api.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RoundAboutNow.Api.Controllers
{
    [Route("api/[controller]")]
    public class StatusController : Controller
    {
        // GET: api/status
        [HttpGet]
        public string Get()
        {
            return "use /api/status/\"latitude\",\"longitude\"";
        }

        // GET api/status/{latitude},{longitude}
        [HttpGet("{latitude},{longitude}")]
        public async Task<StatusMessage> Get(string latitude, string longitude)
        {
            //TODO: metodanrop för get status logik
            var message = await StatusMessageCreator.GetGeneralStatusMessageAsync(latitude, longitude);

            return message;
        }
    }
}
