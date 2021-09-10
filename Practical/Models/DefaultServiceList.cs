using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practical.Models
{
    public class DefaultServiceList
    {
        private static readonly List<string> Services = new List<string>
        {
            "geoIP", "Ping", "RDAP", "Reverse DNS"
        };
        public List<string> DefaultList => Services;

    }
}
