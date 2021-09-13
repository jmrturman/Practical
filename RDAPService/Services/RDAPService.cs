using Data.Models;
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

        public RDAPService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<PracticalResult> LookUp(string ipAddress)
        {
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"RDAP Results for IPAddress: {ipAddress}");
            var alteredIPAddress = "";
            if (!ipAddress.Contains(Uri.SchemeDelimiter))
            {
                alteredIPAddress = string.Concat(Uri.UriSchemeHttp, Uri.SchemeDelimiter, ipAddress);
            }
            Uri uri = new Uri(alteredIPAddress);
            string domain = ipAddress;//uri.Host; // will return www.foo.com
            if (domain.Contains(".org"))
            {
                string url = "https://rdap.publicinterestregistry.net/rdap/org/domain/" + ipAddress;
                practicalResult = await Task.FromResult(GetPracticalResults(url, interemResults));
            }else if (domain.Contains(".com"))
            {
                var url = "https://rdap.verisign.com/com/v1/domain/" + ipAddress;
                practicalResult = await Task.FromResult(GetPracticalResults(url, interemResults));
            }
            else
            {
                interemResults.Add($"{ipAddress} Appears to be an ip address and does not contain TLD .com or .org");
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
