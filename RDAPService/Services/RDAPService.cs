using Data.Models;
using GeoIP.Interfaces;
using GeoIP.Services;
using RDAP.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RDAP.Services
{
    public class RDAPService : IRDAPService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IGeoIPService _geoIPService;

        public RDAPService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _geoIPService = new GeoIPService();
        }
        public async Task<PracticalResult> LookUp(string ipAddress, PracticalResult result)
        {
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"RDAP Results for IPAddress: {ipAddress}");
            var alteredIPAddress = "";
            var name = "";
            if (result.Domain.Equals("ipaddress"))
            {
                IPAddress hostIPAddress = IPAddress.Parse(ipAddress);
                //Obsolete IPHostEntry hostInfo = Dns.GetHostByAddress(hostIPAddress);
                IPHostEntry hostEntry = Dns.GetHostEntry(hostIPAddress);
                name = hostEntry.HostName;
                var hstName = name.Split('.');
                name = hstName[1];
            }         
            if (result.IPAddress.Equals("domain"))
            {
                if (ipAddress.Contains(".org"))
                {
                    string url = "https://rdap.publicinterestregistry.net/rdap/org/domain/" + ipAddress;
                    practicalResult = await Task.FromResult(GetPracticalResults(url, interemResults));
                }
                if (ipAddress.Contains(".com"))
                {
                    string url = "https://rdap.verisign.com/com/v1/domain/" + ipAddress;
                    practicalResult = await Task.FromResult(GetPracticalResults(url, interemResults));
                }
            }           
            else
            {
                string url = "https://rdap.publicinterestregistry.net/rdap/org/domain/" + name + ".org";
                practicalResult = await Task.FromResult(GetPracticalResults(url, interemResults));
                if(interemResults.Count == 1)
                {
                    url = "https://rdap.verisign.com/com/v1/domain/" + ipAddress + ".com";
                    var practicalResultCom = await Task.FromResult(GetPracticalResults(url, interemResults));
                }           
            }
       
            practicalResult.IndividualResults = interemResults;
            return practicalResult;               
        }

        private PracticalResult GetPracticalResults(string url, List<string> interemResults)
        {
            PracticalResult practicalResult = new PracticalResult();
            //credit: https://stackoverflow.com/questions/4327629/get-user-location-by-ip-address
            //https://stackoverflow.com/questions/37101250/how-to-successfully-use-rdap-protocol-instead-of-whois

            
            var request = System.Net.WebRequest.Create(url);

            using (WebResponse wrs = request.GetResponse())
            {
                using Stream stream = wrs.GetResponseStream();
                using StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();                
                interemResults.Add($"Raw JSON from RDAP: {{ {json} }} ");
            }
            
            return practicalResult;
        
        }   
    }

    //endpoint documentation for reference
    //Authority's RDAP API endpoint of the top-level domain .org is 
    //https://rdap.publicinterestregistry.net/rdap/org/. For example 
    //W3C's domain is described at https://rdap.publicinterestregistry.net/rdap/org/domain/w3c.org
    //https://rdap.verisign.com/com/v1/domain/google.com
    //https://rdap.verisign.com/com/v1/domain/w3c.org
}
