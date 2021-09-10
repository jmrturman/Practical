using Microsoft.VisualStudio.TestTools.UnitTesting;
using Practical.Models;
using Practical.Services;
using Practical.Services.Interfaces;


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
