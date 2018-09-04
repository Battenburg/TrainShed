using BaseClients;
using SchedulingClients;
using System.Collections.Generic;
using SchedulingClients.MapServiceReference;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TrainShed.Core
{
    public static class IMapClient_ExtensionMethods
    {
        public static Random random = new Random();

        public static NodeData GetRandomNode(this IEnumerable<NodeData> nodeDataSet, string regexString = null, IEnumerable<int> excluding = null)
        {
            if (nodeDataSet == null || !nodeDataSet.Any())
            {
                return null; 
            }

            if (excluding == null)
            {
                excluding = Enumerable.Empty<int>();
            }

            List<NodeData> pool = new List<NodeData>();

            if (!string.IsNullOrEmpty(regexString))
            {
                Regex regex = new Regex(regexString);
                pool = nodeDataSet.Where(e => regex.IsMatch(e.Alias) && !excluding.Contains(e.MapItemId)).ToList();
            }
            else
            {
                pool = nodeDataSet.Where(e => !excluding.Contains(e.MapItemId)).ToList();
            }

            if (pool.Any())
            {
                return pool.ElementAt(random.Next(pool.Count));
            }

            return null;
        }
        public static IEnumerable<NodeData> GetAllNodeData(this IMapClient mapClient)
        {
            IEnumerable<NodeData> nodeDataSet;
            ServiceOperationResult result = mapClient.TryGetAllNodeData(out nodeDataSet);

            if (result.IsSuccessfull)
            {
                return nodeDataSet;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }

        public static IEnumerable<MoveData> GetAllMoveData(this IMapClient mapClient)
        {
            IEnumerable<MoveData> moveDataSet;
            ServiceOperationResult result = mapClient.TryGetAllMoveData(out moveDataSet);

            if (result.IsSuccessfull)
            {
                return moveDataSet;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }
    }
}
