using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RoundAboutNow.Api.Models.Api.SMHI
{
    public class SMHIWarningsApi
    {
        private string Latitude { get; set; }
        private string Longitude { get; set; }
        private string url = "http://opendata-download-warnings.smhi.se/api/alerts.json";

        public string GetDistrictIdSMHI()
        {
            var latitudeDouble = Convert.ToDouble(Latitude, CultureInfo.InvariantCulture);
            var longitudeDouble = Convert.ToDouble(Longitude, CultureInfo.InvariantCulture);


            foreach (var district in SMHIDistrictHandler.DistrictList)
            {
                var latMax = district.PolygonCoordinates.Max(c => c.Latitude);
                var latMin = district.PolygonCoordinates.Min(c => c.Latitude);
                var longMax = district.PolygonCoordinates.Max(c => c.Longitude);
                var longMin = district.PolygonCoordinates.Min(c => c.Longitude);

                bool isInside = false;

                // Snabbcheck om koordinat ligger innanför boxen som avgörs av min och maxvärde av distrikten
                if (latitudeDouble < latMax && latitudeDouble > latMin && longitudeDouble < longMax && longitudeDouble > longMin)
                {
                    var polygon = district.PolygonCoordinates;

                    // Magic. Teorin bakom: Man ritar en linje från punkten ifråga och en punkt utanför polygonet. Sen kollar man om
                    // den linjen skär en kant av polygonet. Man loopar genom alla kanter. Hur mångar korsar det finns avgör om
                    // punkten ligger innanför polygonet: Ojämt -> innanför, jämt -> utanför
                    for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
                    {
                        if (((polygon[i].Latitude > latitudeDouble) != (polygon[j].Latitude > latitudeDouble)) &&
                            (longitudeDouble < (polygon[j].Longitude - polygon[i].Longitude)
                            * (latitudeDouble - polygon[i].Latitude)
                            / (polygon[j].Latitude - polygon[i].Latitude) + polygon[i].Longitude))
                        {
                            isInside = !isInside;
                        }
                    }
                }

                if (isInside)
                {
                    return district.Id;
                }
            }

            return " NO!";
        }

        public async Task<SMHIWarning> GetWarningsAsync(string latitude, string longitude)
        {
            Latitude = latitude;
            Longitude = longitude;

            string district = GetDistrictIdSMHI();

            var handler = new WebServiceHandler();
            var requestResult = await handler.GetResultFromAPIAsync(url);

            var smhiWarning = new SMHIWarning();
            //TEST PURPOSE USING MOCK TEST DATA
            if (true)
            {
                #region testJsonstring

                requestResult = @"{
	""alert"": [
		{
			""identifier"": ""smhi-bpm-1480440396515"",
			""sender"": ""noreplyvarning@smhi.se"",
			""sent"": ""2016-11-29T18:30:12+01:00"",
			""status"": ""Actual"",
			""msgType"": ""Alert"",
			""scope"": ""Public"",
			""code"": [
				""system_affiliation 5"",
				""latest_update 2016-11-30T02:47:05+01:00"",
				""system_version 2"",
				""system_alert_category warning""
			],
			""info"": {
				""language"": ""sv-SE"",
				""category"": ""Met"",
				""event"": ""average wind speed at sea"",
				""urgency"": ""Immediate"",
				""severity"": ""Extreme"",
				""certainty"": ""Likely"",
				""eventCode"": [
					{
						""valueName"": ""system_event_level"",
						""value"": ""Warning class 1 moderate gale""
					},
					{
						""valueName"": ""system_event_sv-SE"",
						""value"": ""Medelvind till havs""
					},
					{
						""valueName"": ""system_event_priority"",
						""value"": 1
					},
					{
						""valueName"": ""system_event_level_sv-SE"",
						""value"": ""Meteorregn""
					},
					{
						""valueName"": ""system_event_level_color"",
						""value"": ""#fdeb1b""
					},
					{
						""valueName"": ""system_event_level_id"",
						""value"": 1
					}
				],
				""effective"": ""2016-11-29T18:30:12+01:00"",
				""onset"": ""2016-11-29T18:30:12+01:00"",
				""expires"": ""2017-11-29T18:26:36+01:00"",
				""senderName"": ""SMHI, Swedish Meteorological and Hydrological Institute"",
				""headline"": ""Bottenviken"",
				""description"": ""Från onsdag förmiddag nordväst 15 m/s. Natt mot torsdag avtagande."",
				""web"": ""http://www.smhi.se/vadret/vadret-i-sverige/Varningar"",
				""parameter"": [
					{
						""valueName"": ""system_position"",
						""value"": 1
					},
					{
						""valueName"": ""system_history"",
						""value"": ""{\""history\"":[{\""date\"":\""2016-11-29T18:30:12\"",\""node\"":\""description\"",\""label\"":\""Områdesbeskrivning (Sv)\"",\""value\"":\""Från onsdag middag nordväst 15 m/s.\""},{\""date\"":\""2016-11-30T02:44:24\"",\""node\"":\""description\"",\""label\"":\""Områdesbeskrivning (Sv)\"",\""value\"":\""Från onsdag förmiddag nordväst 15 m/s. Natt mot torsdag avtagande.\""}]}""
					},
					{
						""valueName"": ""system_navtex"",
						""value"": ""{\""status\"":\""active\"",\""svdescription\"":\""FRÅN ONSDAG FÖRMIDDAG NORDVÄST 15 M/S. NATT MOT TORSDAG AVTAGANDE.\"",\""endescription\"":\""FROM WEDNESDAY MORNING NW 15 M/S. NIGHT TOWARDS THURSDAY DECR.\""}""
					},
					{
						""valueName"": ""system_eng_headline"",
						""value"": ""No English headline available""
					},
					{
						""valueName"": ""system_eng_description"",
						""value"": ""No English description available""
					}
				],
				""area"": {
					""areaDesc"": ""025""
				}
			}
		}]";
                #endregion // testJsonstring
            }
            try
            {
                var wrapper = new
                {
                    alert = new List<SingleWarning>()
                };

                var alerts = JsonConvert.DeserializeAnonymousType(requestResult, wrapper);

                foreach (var alert in alerts.alert)
                {
                    if (alert.info.Area["areaDesc"] == district)
                    {
                        smhiWarning.WarningClass = GetWarningClass(alert.info.Severity);
                        smhiWarning.WeatherWarningMessage = alert.info.EventCode[3]["value"];
                    }
                }
            }
            catch (Exception ex)
            {

                var error = ex.Message;
            }




            return smhiWarning;

            //var allTheJson = JsonConvert.DeserializeObject
            //            <List<
            //                Dictionary<
            //                    string, Dictionary<
            //                        string, List<
            //                            Dictionary<
            //                                string, string>>>>>>
            //                                (requestResult);
        }

        private int GetWarningClass(string severity)
        {
            if (severity == "Moderate")
            {
                return 1;
            }
            else if (severity == "")
            {
                return 2;
            }
            else if (severity == "Extreme")
            {
                return 3;
            }
            else
            {
                return 0;
            }
        }
    }
}
