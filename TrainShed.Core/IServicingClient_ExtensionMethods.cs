using BaseClients;
using SchedulingClients;

namespace TrainShed.Core
{
    public static class IServicingClient_ExtensionMethods
    {
        public static bool SetServiceComplete(this IServicingClient servicingClient, int taskId)
        {
            bool succcess;
            ServiceOperationResult result = servicingClient.TrySetServiceComplete(taskId, out succcess);

            if (result.IsSuccessfull)
            {
                return succcess;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }
    }
}
