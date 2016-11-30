using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public class SMHIWarning
    {
        public int WarningClass { get; set; }
        public string WeatherWarningMessage { get; set; }
    }
}
