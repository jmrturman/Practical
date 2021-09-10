using System.Collections.Generic;
using System.Net;

namespace Data.Models
{
    public class PracticalResult
    {
        public IEnumerable<string> IndividualResults { get; set; }
        public IEnumerable<IPAddress> IPAddresses { get; set; }

    }
}
