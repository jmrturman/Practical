using System.Collections.Generic;
using System.Net;

namespace Data.Models
{
    public class PracticalResult
    {
        public string Name { get; set; }
        public IEnumerable<string> IndividualResults { get; set; }
        public IEnumerable<IPAddress> IPAddresses { get; set; }

        public string IPAddress { get; set; }

        public string Domain { get; set; }

    }
}
