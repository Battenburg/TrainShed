using BaseClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainShed.Core
{
    public static class Tools
    {
        public static Exception GetException(ServiceOperationResult result)
        {
            if (result.IsClientError)
            {
                return result.ClientException;
            }
            else if (result.IsServiceError)
            {
                return result.ServiceException;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
