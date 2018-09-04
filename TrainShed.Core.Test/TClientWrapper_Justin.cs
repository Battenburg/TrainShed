using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using BaseClients;
using System.Net;
using SchedulingClients.JobBuilderServiceReference;
using SchedulingClients.MapServiceReference;
using System.Diagnostics;
using SchedulingClients.ServicingServiceReference;
using System.Threading;

namespace TrainShed.Core.Test
{
    [TestFixture]
    [Category("Justin")]
    public class TClientWrapper_Justin
    {
        private EndpointSettings endpointSettings;
  
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.endpointSettings = new EndpointSettings(IPAddress.Loopback);
        }

        /// <summary>
        /// Creates a pick at a random node with 'pick' in the alias.
        /// </summary>
        [Test]
        public void CreateJob()
        {
            int serviceTaskId = -1;
            AutoResetEvent serviceReady = new AutoResetEvent(false); // Just using this to wait for vehicle at node. 

            using (ClientWrapper clientWrapper = new ClientWrapper())
            {
                clientWrapper.Configure(endpointSettings);

                // Inline delegate just for demo. Don't shoot the messenger. 
                // Here we just tell the servicing client we have finished interracting with the vehicle and
                // it can do other jobs. 
                clientWrapper.ServicingClient.ServiceRequest += delegate(ServiceStateData serviceStateData)
                {
                    if (serviceTaskId >= 0 && serviceStateData.TaskId == serviceTaskId)
                    {
                        // This is basically acknowledging the vehicle is at the node, so we can release the tote
                        clientWrapper.ServicingClient.SetServiceComplete(serviceTaskId);
                        serviceReady.Set(); 
                    }
                };

                JobData jobData = clientWrapper.JobBuilderClient.CreateJob();               

                // Creating the pick task
                NodeData pickNode = clientWrapper.NodeCache.GetRandomNode(RegexFactory.ContainsSubStringCaseInvariant("A-Pickup"));

                // We are assumming the vehicle is at the node, but it might not be, so just in case lets just make sure it is.
                // Create a service task 
                serviceTaskId = clientWrapper.JobBuilderClient.CreateManualLoadHandling(jobData.RootOrderedListTaskId, pickNode.MapItemId);

                // Now lets create an actual receive task, with a '4' to passively receive a load
                int picktaskId = clientWrapper.JobBuilderClient.CreateExecution(jobData.RootOrderedListTaskId, pickNode.MapItemId);
                clientWrapper.JobBuilderClient.IssueDirective(picktaskId, "DockType", (byte)4 );                               

                // Creating the drop task
                NodeData dropNode = clientWrapper.NodeCache.GetRandomNode(RegexFactory.ContainsSubStringCaseInvariant("Dropoff"));
                int dropTaskId = clientWrapper.JobBuilderClient.CreateExecution(jobData.RootOrderedListTaskId, dropNode.MapItemId);
                // '3' is passively drop a load
                clientWrapper.JobBuilderClient.IssueDirective(dropTaskId, "DockType", (byte)3);

                // It is ideal to wait at the pick node - sends the vehicle back to the pick node. The job will be complete once it is here
                // so if it has to do something else (like charge) it is free to get other jobs.
                clientWrapper.JobBuilderClient.CreateMovingTask(jobData.RootOrderedListTaskId, pickNode.MapItemId);

                clientWrapper.JobBuilderClient.Commit(jobData.JobId);

                if (!serviceReady.WaitOne(TimeSpan.FromSeconds(20)))
                {
                    throw new TimeoutException("timeout on waiting for vehicle at the pick node");
                }
            }
        }
    }
}
