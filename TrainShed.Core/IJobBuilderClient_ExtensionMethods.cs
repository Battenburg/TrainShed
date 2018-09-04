using SchedulingClients.JobBuilderServiceReference;
using SchedulingClients;
using BaseClients;
using System;
using System.Net;

namespace TrainShed.Core
{
    public static class IJobBuilderClient_ExtensionMethods
    {
        public static int CreateManualLoadHandling(this IJobBuilderClient jobBuilder, int parentListTaskId, int nodeId, TimeSpan expectedDuration = default(TimeSpan))
        {
            int serviceTaskId;
            ServiceOperationResult result = jobBuilder.TryCreateServicingTask(parentListTaskId, nodeId, ServiceType.ManualLoadHandling, out serviceTaskId, expectedDuration);

            if (result.IsSuccessfull)
            {
                return serviceTaskId;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }

        public static void IssueDirective(this IJobBuilderClient jobBuilder, int taskId, string parameterAlias, ushort value)
        {
            ServiceOperationResult result = jobBuilder.TryIssueDirective(taskId, parameterAlias, value);

            if (!result.IsSuccessfull)
            {
                throw Tools.GetException(result);
            }
        }

        public static void IssueDirective(this IJobBuilderClient jobBuilder, int taskId, string parameterAlias, IPAddress ipAddress)
        {
            ServiceOperationResult result = jobBuilder.TryIssueDirective(taskId, parameterAlias, ipAddress);

            if (!result.IsSuccessfull)
            {
                throw Tools.GetException(result);
            }
        }

        public static void IssueDirective(this IJobBuilderClient jobBuilder, int taskId, string parameterAlias, short value)
        {
            ServiceOperationResult result = jobBuilder.TryIssueDirective(taskId, parameterAlias, value);

            if (!result.IsSuccessfull)
            {
                throw Tools.GetException(result);
            }
        }

        public static void IssueDirective(this IJobBuilderClient jobBuilder, int taskId, string parameterAlias, float value)
        {
            ServiceOperationResult result = jobBuilder.TryIssueDirective(taskId, parameterAlias, value);

            if (!result.IsSuccessfull)
            {
                throw Tools.GetException(result);
            }
        }

        public static void IssueDirective(this IJobBuilderClient jobBuilder, int taskId, string parameterAlias, byte value)
        {
            ServiceOperationResult result = jobBuilder.TryIssueDirective(taskId, parameterAlias, value);

            if (!result.IsSuccessfull)
            {
                throw Tools.GetException(result);
            }
        }

        public static int CreateExecution(this IJobBuilderClient jobBuilder, int parentListTaskId, int nodeId, TimeSpan expectedDuration = default(TimeSpan))
        {
            int executionTaskId;
            ServiceOperationResult result = jobBuilder.TryCreateServicingTask(parentListTaskId, nodeId, ServiceType.Execution, out executionTaskId, expectedDuration);

            if (result.IsSuccessfull)
            {
                return executionTaskId;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }

        public static JobData CreateJob(this IJobBuilderClient jobBuilder)
        {
            JobData jobData;
            ServiceOperationResult result = jobBuilder.TryCreateJob(out jobData);

            if (result.IsSuccessfull)
            {
                return jobData;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }

        public static bool Commit(this IJobBuilderClient jobBuilder, int jobId)
        {
            bool success;
            ServiceOperationResult result = jobBuilder.TryCommit(jobId, out success);

            if (result.IsSuccessfull)
            {
                return success;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }


        public static int CreateMovingTask(this IJobBuilderClient jobBuilder, int listTaskId, int nodeId)
        {
            int moveTaskId;
            ServiceOperationResult result = jobBuilder.TryCreateMovingTask(listTaskId, nodeId, out moveTaskId);

            if (result.IsSuccessfull)
            {
                return moveTaskId;
            }
            else
            {
                throw Tools.GetException(result);
            }
        }
    }
}
