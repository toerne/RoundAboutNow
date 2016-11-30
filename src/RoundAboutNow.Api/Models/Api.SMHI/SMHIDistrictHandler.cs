using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public static class SMHIDistrictHandler
    {
        private static string JsonAnswerDistricts { get; set; }
        public static string JsonAnswerWarnings { get; set; }
        public static List<SMHIDistrict> DistrictList { get; set; }

        static SMHIDistrictHandler()
        {

            JsonAnswerDistricts = File.ReadAllLines("SMHIDistrictsAnswer.txt")[0];

            DistrictList = new List<SMHIDistrict>();
            var wrapper = new
            {
                district = new List<SMHIDistrict>()
            };

            var result = JsonConvert.DeserializeAnonymousType(JsonAnswerDistricts, wrapper);

            foreach (var district in result.district)
            {
                DistrictList.Add(district);
                // Parsar koordinaterna

                var coordinateString = district.Geometry.exact_polygon.Substring(9, district.Geometry.exact_polygon.Length - 11);
                List<string> coordinateList = coordinateString.Split(',').ToList();

                foreach (var coordinateElement in coordinateList)
                {
                    string[] coordinatePart = coordinateElement.Split(' ');

                    district.PolygonCoordinates.Add(new SMHICoordinate
                    {
                        Latitude = Convert.ToDouble(coordinatePart[0], CultureInfo.InvariantCulture),
                        Longitude = Convert.ToDouble(coordinatePart[1], CultureInfo.InvariantCulture)
                    });
                }
            }

        }
    }
}
