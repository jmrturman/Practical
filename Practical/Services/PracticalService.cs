using Practical.Models;
using Practical.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Services
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
            foreach(var service in services)
            {
                switch (service)
                {
                    case "Reverse DNS":
                        resultList.Add(await _reverseDNSService.LookUp(ipAddress));
                        break;
                    case "geoIP":
                        resultList.Add(await _geoIPService.LookUp(ipAddress));
                        break;
                    case "RDAP":
                        resultList.Add(await _rDAPService.LookUp(ipAddress));
                        break;
                    case "Ping":
                        resultList.Add(await _pingService.LookUp(ipAddress));
                        break;
                    default:
                        break;
                }
            }
            
            return resultList;
        }

    }
}
