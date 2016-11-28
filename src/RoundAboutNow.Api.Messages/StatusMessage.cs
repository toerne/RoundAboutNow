using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Messages
{
    public class StatusMessage
    {
        public int WarningLevel { get; set; }
        public string WarningMessage { get; set; }
        public string Location { get; set; }
        public List<StationCoordinate> Stations { get; set; } = new List<StationCoordinate>();
    }
}
