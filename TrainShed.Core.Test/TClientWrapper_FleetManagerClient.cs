using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BaseClients;
using System.Net;

namespace TrainShed.Core.Test
{
    [TestFixture]
    public class TClientWrapper_FleetManagerClient
    {
        private EndpointSettings endpointSettings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            endpointSettings = new EndpointSettings(IPAddress.Loopback);
        }

        [Test]
        [TestCase("192.168.66.100")]
        public void CreateVirtualVehicleAtOrigin(string ipV4String)
        {
            IPAddress ipAddress = IPAddress.Parse(ipV4String);

            using (ClientWrapper clientWraper = new ClientWrapper())
            {
                clientWraper.Configure(endpointSettings);
                clientWraper.FleetManagerClient.CreateVirtualVehicleAtOrigin(ipAddress);
            }
        }
    }
}
