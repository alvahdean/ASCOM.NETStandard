using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RACI.Client;
using RACI.Data;
using System;

namespace MSTest.RaciClientTests
{
    [TestClass]
    public class ClientTests
    {
        private string _url = "https://localhost:44378/rascom";
        [TestMethod]
        public void CheckConnectionTest()
        {
            var result=ConnectionCheck.Run(_url);
            Console.WriteLine(result);
            Assert.IsTrue(result.AvgResponseTime > .50, "Check connection queality >50%");
            result = ConnectionCheck.Run(_url+"/fake");
            Console.WriteLine(result);
            Assert.IsNull(result.AvgResponseTime, "Check connection queality 0%");
        }

        [TestMethod]
        public void PingService()
        {
            bool result =RaciClient.PingService(_url);
            Assert.IsTrue(result, "Check ping returns true");
        }

        [TestMethod]
        public void DiscoverService()
        {
            RaciEndpoint ep= RaciClient.QueryEndpoint(_url);

            Assert.IsNotNull(ep, "Check returned endpoint is not null");
            Assert.IsTrue(ep.Nodes.Count > 0, "Check endpoint has at least one registered driver");
            string json = JsonConvert.SerializeObject(ep,Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
