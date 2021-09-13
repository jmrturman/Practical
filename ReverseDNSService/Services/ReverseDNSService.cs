using Data.Models;
using GeoIP.Interfaces;
using GeoIP.Services;
using ReverseDNS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ReverseDNS.Services
{
    public class ReverseDNSService : IReverseDNSService
    {
        private readonly IGeoIPService _geoIPService;

        public ReverseDNSService()
        {
            _geoIPService = new GeoIPService();
        }
        public async Task<PracticalResult> LookUp(string ipAddress, PracticalResult result)
        {
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"ReverseDNS Results for IPAddress: {ipAddress}");
            try
            {
                if (result.IPAddress.Equals("domain"))
                {
                    ipAddress = await GetIPAddressForDomain(ipAddress, interemResults);
                }
                //credit https://stackoverflow.com/questions/716748/reverse-ip-domain-check
                IPAddress hostIPAddress = IPAddress.Parse(ipAddress);
                //Obsolete IPHostEntry hostInfo = Dns.GetHostByAddress(hostIPAddress);
                IPHostEntry hostEntry = Dns.GetHostEntry(hostIPAddress);               
                practicalResult = await Task.FromResult(GetPracticalResult(hostEntry, interemResults));
            }
            catch(Exception ex)
            {
                interemResults.Add($"There was an issue during reverse dns processing: {ex.Message}");
                practicalResult.IndividualResults = interemResults;
            }           
            return practicalResult;
        }

        private async Task<string> GetIPAddressForDomain(string ipAddress, List<string> interemResults)
        {
            PracticalResult result = new PracticalResult();
            var ipFromDomain = "";
            result = await _geoIPService.LookUp(ipAddress, result);
            if(result != null)
            {
                ipFromDomain = result.IPAddress;
            }
            return ipFromDomain;        
        }

        private string GetAliases(IEnumerable<string> aliases)
        {
            string result = "Aliases: ";
            result = String.Concat(String.Join(',', aliases.ToList()));
            return result;
        }

        private string GetIPAddresses(IEnumerable<IPAddress> ipAddresses)
        {
            var result = new System.Text.StringBuilder();
            result.Append("IPAddresses: ");

            foreach (var address in ipAddresses)
            {
                result.Append(address.ToString());
                result.Append(", ");
            }             
            return result.ToString();
        }

        private PracticalResult GetPracticalResult(IPHostEntry hostEntry, List<string> interemResults)
        {
            PracticalResult practicalResult = new PracticalResult();
            var hostName = hostEntry.HostName;
            interemResults.Add($"HostName: {hostName}");
            interemResults.Add(GetAliases(hostEntry.Aliases.ToList()));
            interemResults.Add(GetIPAddresses(hostEntry.AddressList.ToList()));
            practicalResult.IndividualResults = interemResults;
            return practicalResult;
        }
    }
}