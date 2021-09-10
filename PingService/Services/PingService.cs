using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using PingIP.Interfaces;
using Data.Models;

namespace PingIP.Services
{
    public class PingService : IPingService
    {
        public async Task<PracticalResult> LookUp(string ipAddress)
        {
            
            Ping pinger = null;
            PracticalResult practicalResult = new PracticalResult();
            List<string> interemResults = new List<string>();
            interemResults.Add($"Ping Results for IPAddress: {ipAddress}");
            try
            {
                pinger = new Ping();                
                practicalResult = await Task.FromResult(GetPracticalResult(pinger, ipAddress, interemResults));
            }
            catch (PingException pex)
            {
                interemResults.Add($"There were issues with the Ping service: {pex.Message}");
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return practicalResult;

        }

        private PracticalResult GetPracticalResult(Ping pinger, string ipAddress, List<string> interemResults)
        {
            PracticalResult practicalResult = new PracticalResult();
            //credit https://stackoverflow.com/questions/11800958/using-ping-in-c-sharp
            PingReply reply = pinger.Send(ipAddress);
            bool pingable = reply.Status == IPStatus.Success;
            var roundTripTime = reply.RoundtripTime.ToString();
            interemResults.Add($"successful ping: {pingable}");
            interemResults.Add($"roundtrip: {roundTripTime}");
            practicalResult.IndividualResults = interemResults;
            return practicalResult;
        }
    }
}
