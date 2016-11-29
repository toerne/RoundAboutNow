using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Messages
{
    public class Station
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int StatusLevel { get; set; }
        public string Name { get; set; }
        public List<Disturbance> Disturbances { get; set; } = new List<Disturbance>();
    }
}
