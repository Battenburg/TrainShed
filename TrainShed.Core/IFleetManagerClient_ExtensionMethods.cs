using System;
using System.Collections.Generic;
using System.Linq;
using FleetClients;
using FleetClients.FleetManagerServiceReference;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using BaseClients;

namespace TrainShed.Core
{
    public static class IFleetManagerClient_ExtensionMethods
    {
        public static bool CreateVirtualVehicleAtOrigin(this IFleetManagerClient fleetManager, IPAddress ipAddress)
        {
            PoseData pose = new PoseData() { X = 0, Y = 0, Heading = 0 };
            return fleetManager.CreateVirtualVehicle(ipAddress, pose);
        }

        public static bool CreateVirtualVehicle(this IFleetManagerClient fleetManager, IPAddress ipAddress, PoseData pose)
        {
            bool success;
            ServiceOperationResult result = fleetManager.TryCreateVirtualVehicle(ipAddress, pose, out success);

            if (result.IsSuccessfull)
            {
                return success;
            }
            else
            {
                throw Tools.GetException(result);
            }
        } 
    }
}
