using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoundAboutNow.Api.Messages;

namespace RoundAboutNow.Models.ViewModels
{
    public class LocationStatusVM
    {
        public string Header { get; set; }
        public string WarningMessage { get; set; }
        public int WarningLevel { get; set; }
        public List<Station> Stations { get; set; }
    }
}
