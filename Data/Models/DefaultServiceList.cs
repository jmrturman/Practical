using System.Collections.Generic;

namespace Data.Models
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
