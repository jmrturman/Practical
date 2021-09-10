using GeoIP.Interfaces;
using GeoIP.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace PracticalUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IGeoIPService _geoIPService;
        public UnitTest1()
        {
            _geoIPService = new GeoIPService();
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
