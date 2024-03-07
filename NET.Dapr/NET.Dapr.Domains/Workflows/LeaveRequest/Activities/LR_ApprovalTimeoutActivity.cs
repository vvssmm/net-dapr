using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using static NET.Dapr.Domains.Consts;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public class LR_ApprovalTimeoutActivity(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork) 
        : WorkflowActivity<LRTimeoutRequest, LRTimeoutResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LR_ApprovalTimeoutActivity>();
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task<LRTimeoutResponse> RunAsync(WorkflowActivityContext context, LRTimeoutRequest input)
        {
            _logger.LogInformation($"WF Instance {context.InstanceId} Access to LRProcessTimeoutTransactionActivity");
            _logger.LogInformation($"WF Instance {context.InstanceId} auto terminate because approver not respond after 10 minutes");

            var leaverRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var leaverRequest = leaverRequestDbSet.FirstOrDefault(x => x.Id == input.TransactionId);
            if (leaverRequest is not null)
            {
                leaverRequest.Status = LRWorkflowStatus.Timeout.ToString();
                var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
                var taskItem = taskDbSet.FirstOrDefault(x => x.TransactionId == input.TransactionId);
                if (taskItem is not null)
                {
                    taskItem.Status = LRTaskStatus.Terminated.ToString();
                }
                await _unitOfWork.SaveChangesAsync();
            }
            throw new NotImplementedException();
        }
    }
    public record LRTimeoutRequest(long TransactionId);
    public record LRTimeoutResponse();
}
