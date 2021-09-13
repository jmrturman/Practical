using Data.Models;
using GeoIP.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GeoIP.Services
{
    public class GeoIPService : IGeoIPService
    {
        public async Task<PracticalResult> LookUp(string ipAddress)
        {
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"GeoIP Results for IPAddress: {ipAddress}");

            try
            {                
                practicalResult = await Task.FromResult(GetPracticalResult(ipAddress, interemResults));
            }
            catch (Exception ex)
            {
                interemResults.Add($"There were issues with the GeoIP service: {ex.Message}");
                practicalResult.IndividualResults = interemResults;
            }
            return practicalResult;
        }

        public PracticalResult GetPracticalResult(string ipAddress, List<string> interemResults)
        {
            PracticalResult practicalResult = new PracticalResult();

            //credit: https://stackoverflow.com/questions/4327629/get-user-location-by-ip-address
            var accessKey = "a40d53a1299a134875027bd903c911c9";
            string url = "http://api.ipstack.com/" + ipAddress + "?access_key=" + accessKey;
            var request = System.Net.WebRequest.Create(url);
            string actualIP = "";
            using (WebResponse wrs = request.GetResponse())
            {
                using Stream stream = wrs.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                var obj = JObject.Parse(json);                
                string City = (string)obj["city"];
                string Country = (string)obj["region_name"];
                actualIP = (string)obj["ip"];
                interemResults.Add($"Country: {Country}");
                interemResults.Add($"City: {City}");
                interemResults.Add($"Raw JSON from geoIP: {{ {json} }} ");
            }
            practicalResult.IndividualResults = interemResults;
            practicalResult.IPAddress = actualIP;
            return practicalResult;
        }


    }
}
