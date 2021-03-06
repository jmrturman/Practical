using Practical.Models;
using Practical.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Practical.Services
{
    public class ReverseDNSService : IReverseDNSService
    {
        public async Task<PracticalResult> LookUp(string ipAddress)
        {
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"ReverseDNS Results for IPAddress: {ipAddress}");
            try
            {
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