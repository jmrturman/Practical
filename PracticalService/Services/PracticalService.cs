using Data.Models;
using GeoIP.Interfaces;
using MainService.Interfaces;
using PingIP.Interfaces;
using RDAP.Interfaces;
using ReverseDNS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainService.Services
{
    public class PracticalService : IPracticalService
    {
        private readonly IReverseDNSService _reverseDNSService;
        private readonly IGeoIPService _geoIPService;
        private readonly IRDAPService _rDAPService;
        private readonly IPingService _pingService;

      
        public PracticalService (IReverseDNSService reverseDNSService, IGeoIPService geoIPService, IRDAPService rDAPService, IPingService pingService)
        {
            _reverseDNSService = reverseDNSService ?? throw new ArgumentNullException(nameof(reverseDNSService));
            _geoIPService = geoIPService ?? throw new ArgumentNullException(nameof(geoIPService));
            _rDAPService = rDAPService ?? throw new ArgumentNullException(nameof(rDAPService));
            _pingService = pingService ?? throw new ArgumentNullException(nameof(pingService));
        }


        public async Task<IEnumerable<PracticalResult>> CallServices(string ipAddress, IEnumerable<string> services)
        {
            List<PracticalResult> resultList = new List<PracticalResult>();
            var result = await GetInitialResult(ipAddress);
           
            resultList.Add(result);
           
            try
            {

                Parallel.ForEach(services, async service =>
                {
                    switch (service)
                    {
                        case "Reverse DNS":
                            resultList.Add(await _reverseDNSService.LookUp(ipAddress, result));
                            break;
                        case "geoIP":
                            resultList.Add(await _geoIPService.LookUp(ipAddress, result));
                            break;
                        case "RDAP":
                            resultList.Add(await _rDAPService.LookUp(ipAddress, result));
                            break;
                        case "Ping":
                            resultList.Add(await _pingService.LookUp(ipAddress, result));
                            break;
                        default:
                            break;
                    }
                });
 
            }
            catch(Exception ex)
            {
                var exceptionPartial = resultList[0];
                var exceptIndividualResult = exceptionPartial.IndividualResults.ToList();
                exceptIndividualResult.Add($"There was an error in one or more services: {ex.Message}");
            }
            
            
            return resultList;
        }

        private async Task<PracticalResult> GetInitialResult(string ipAddress)
        {
            PracticalResult result = new PracticalResult();
            result.Name = "Calling Services";
            if (ipAddress.Contains(".org"))
            {
                result.Domain = ipAddress;
                result.IPAddress = "domain";
            }
            else if (ipAddress.Contains(".com"))
            {
                result.Domain = ipAddress;
                result.IPAddress = "domain";
            }
            else
            {
                result.IPAddress = ipAddress;
                result.Domain = "ipaddress";

            }
            return result;
        }
    }
}
