using RoundAboutNow.Api.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public class SMHIDistrict
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SMHICoordinate> PolygonCoordinates { get; set; } = new List<SMHICoordinate>();
        public Geometry Geometry { get; set; }
    }
}
