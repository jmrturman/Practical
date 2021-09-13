using Data.Models;
using PracticalValidation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PracticalValidation.Services
{
    public class PracticalValidationService : IPracticalValidationService
    {
        public string Validate(string ipAddress, out bool isValid)
        {
            isValid = false;
            string isValidInput = "A Valid Address Must Be Supplied";
            if (ipAddress.Contains(".org") || ipAddress.Contains(".com"))
            {
                ValidateDomain(ipAddress, out isValidInput, out isValid);
            }
            else
            {
                var splitAddress = ipAddress.Split('.');
                var firstPositin = splitAddress[0];
                var tst = Int32.TryParse(firstPositin, out _);
                if (!(String.IsNullOrEmpty(firstPositin) && Int32.TryParse(firstPositin, out _)))
                {
                    ValidateIP(ipAddress, out isValidInput, out isValid);
                }
            }
            
            return isValidInput;
        }

        public string Validate(IEnumerable<string> serviceList, out bool isValid)
        {
            isValid = false;
            string isValidInput = "A Valid Service List Must Be Supplied";
            try
            {
                DefaultServiceList defaults = new DefaultServiceList();
                List<string> found = new List<string>();

                found.AddRange(defaults.DefaultList.Select(i => i.ToString()).Intersect(serviceList));

                if (found.Count > 0)
                {
                    isValidInput = "Valid Service List";
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                isValidInput = $"There was a problem validating the service list: {ex.Message}";
            }            
            
            return isValidInput;
        }
        private void ValidateDomain(string ipAddress, out string isValidInput, out bool isValid)
        {
            isValid = false;
            isValidInput = "A Valid Domain Must Be Supplied";
            try
            {
                //https://stackoverflow.com/questions/967516/best-way-to-determine-if-a-domain-name-would-be-a-valid-in-a-hosts-file
                if (Uri.CheckHostName(ipAddress) != UriHostNameType.Unknown)
                {
                    isValidInput = "Valid Domain";
                    isValid = true;
                }
            }
            catch(Exception ex)
            {
                isValidInput = $"There was an issue validating the Domain: {ex.Message}";
            }
           
        }
        private void ValidateIP(string ipAddress, out string isValidInput, out bool isValid)
        {
            isValid = false;
            isValidInput = "A Valid IP Must Be Supplied";
            try
            {
                //https://stackoverflow.com/questions/799060/how-to-determine-if-a-string-is-a-valid-ipv4-or-ipv6-address-in-c
                //if (!ipAddress.Contains(Uri.SchemeDelimiter))
                //{
                //    ipAddress = string.Concat(Uri.UriSchemeHttp, Uri.SchemeDelimiter, ipAddress);
                //}
                if (IPAddress.TryParse(ipAddress, out IPAddress address))
                {
                    switch (address.AddressFamily)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            isValidInput = "Valid IPv4";
                            isValid = true;
                            break;
                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            isValidInput = "Valid IPv6";
                            isValid = true;
                            break;
                        default:

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                isValidInput = $"There was an issue validating the Domain: {ex.Message}";
            }
        }
    }
}
