using BaseClients;
using FleetClients;
using SchedulingClients;
using SchedulingClients.MapServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using FC = FleetClients;
using SC = SchedulingClients;

namespace TrainShed.Core
{
    public class ClientWrapper : IDisposable
    {
        private List<IClient> clients = new List<IClient>();

        private bool isDisposed = false;

        public ClientWrapper()
        {
            readOnlyNodeCache = new ReadOnlyObservableCollection<NodeData>(nodeCache);
        }

        ~ClientWrapper()
        {
            Dispose(false);
        }

        public List<IClient> Clients => clients;

        public IFleetManagerClient FleetManagerClient => GetClient<IFleetManagerClient>();

        public IJobBuilderClient JobBuilderClient => GetClient<IJobBuilderClient>();

        public IMapClient MapClient => GetClient<IMapClient>();

        public IServicingClient ServicingClient => GetClient<IServicingClient>();

        public bool IsConfigured() => clients.Any();

        public void Configure(EndpointSettings endpointSettings)
        {
            if (IsConfigured())
            {
                throw new InvalidOperationException("Already configured");
            }

            lock (clients)
            {
                clients.Add(FC.ClientFactory.CreateTcpFleetManagerClient(endpointSettings));

                clients.Add(SC.ClientFactory.CreateTcpJobBuilderClient(endpointSettings));
                clients.Add(SC.ClientFactory.CreateTcpMapClient(endpointSettings));
                clients.Add(SC.ClientFactory.CreateTcpServicingClient(endpointSettings));
            }

            UpdateMapCache();
        }

        private ObservableCollection<MoveData> moveCache = new ObservableCollection<MoveData>();

        private ReadOnlyObservableCollection<MoveData> readOnlyMoveCache;

        public ReadOnlyObservableCollection<MoveData> MoveCache => readOnlyMoveCache;

        private ObservableCollection<NodeData> nodeCache = new ObservableCollection<NodeData>();

        private ReadOnlyObservableCollection<NodeData> readOnlyNodeCache;

        public ReadOnlyObservableCollection<NodeData> NodeCache => readOnlyNodeCache;

        public void UpdateMapCache()
        {
            nodeCache.Clear();
            moveCache.Clear();

            foreach(NodeData nodeData in  MapClient != null ? MapClient.GetAllNodeData() : Enumerable.Empty<NodeData>())
            {
                nodeCache.Add(nodeData);
            }

            foreach (MoveData moveData in MapClient != null ? MapClient.GetAllMoveData() : Enumerable.Empty<MoveData>())
            {
                moveCache.Add(moveData);
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposed)
            {
                return;
            }

            foreach (IClient client in clients)
            {
                client.Dispose();
            }

            isDisposed = true;
        }

        private T GetClient<T>()
        {
            return clients.Where(e => e is T).Cast<T>().FirstOrDefault();
        }
    }
}