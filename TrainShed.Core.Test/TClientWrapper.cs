using NUnit.Framework;
using BaseClients;
using System.Net;

namespace TrainShed.Core.Test
{
    [TestFixture]
    public class TClientWrapper
    {
        [Test]
        public void Init()
        {
            using (ClientWrapper clientWrapper = new ClientWrapper())
            {
                Assert.IsNull(clientWrapper.JobBuilderClient);
                Assert.IsNull(clientWrapper.MapClient);
                Assert.IsNull(clientWrapper.ServicingClient);
            }
        }

        [Test]
        public void Configure()
        {
            using (ClientWrapper clientWrapper = new ClientWrapper())
            {
                clientWrapper.Configure(new EndpointSettings(IPAddress.Loopback));

                Assert.IsNotNull(clientWrapper.JobBuilderClient);
                Assert.IsNotNull(clientWrapper.MapClient);
                Assert.IsNotNull(clientWrapper.ServicingClient);
            }
        }
    }
}
