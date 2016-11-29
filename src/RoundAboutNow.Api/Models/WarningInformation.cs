﻿using RoundAboutNow.Api.Messages;
using RoundAboutNow.Api.Models.Api.SL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models
{
    public class WarningInformation
    {
        private string warningMessage = "";
        private int warningLevel = 1;
        public List<SLStation> SLStations { get; set; } = new List<SLStation>();


        private void CreateWarning()
        {
            var percentage = CalculatePercentageOfDisturbedStation();
            if (percentage < 0.25)
            {
                warningLevel = 1;
            }
            else if (percentage < 0.75)
            {
                warningLevel = 2;
            }
            else
            {
                warningLevel = 3;
            }

            warningMessage += $"{SLStations.Where(s => s.Disturbances.Count > 0).Count()} av {SLStations.Count} ({Math.Round(percentage * 100, 0)}%) hållplatser rapporterar störningar";
        }

        public void AppendWarningInformation(StatusMessage message)
        {
            CreateWarning();
            message.WarningLevel = warningLevel;
            message.WarningMessage = warningMessage;
            foreach (var station in SLStations)
            {
                var newStation = new Station
                {
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    StatusLevel = station.Disturbances.Count() > 0 ? 1 : 0,
                    Name = station.Name
                };

                foreach (var disturbance in station.Disturbances)
                {
                    newStation.Disturbances.Add(new Disturbance { Header = disturbance.Header, Details = disturbance.Details });
                }

                message.Stations.Add(newStation);
            }
        }

        private double CalculatePercentageOfDisturbedStation()
        {
            return SLStations.Count > 0 ? (double)SLStations.Where(s => s.Disturbances.Count > 0).Count() / SLStations.Count() : 0;
        }
    }
}
