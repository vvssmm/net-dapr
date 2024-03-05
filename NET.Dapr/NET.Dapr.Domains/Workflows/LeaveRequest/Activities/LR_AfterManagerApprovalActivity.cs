using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using System.Text.Json;
using static NET.Dapr.Domains.Workflows.Consts;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public class LR_AfterManagerApprovalActivity(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork) : WorkflowActivity<LRProcessApproveTransactionRequest, LRProcessApproveTransactionResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LR_AfterManagerApprovalActivity>();
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task<LRProcessApproveTransactionResponse> RunAsync(WorkflowActivityContext context, LRProcessApproveTransactionRequest input)
        {
            _logger.LogInformation($"WF instance {context.InstanceId} to LRProcessApproveTransactionActivity");
            var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
            var taskItem =taskDbSet.FirstOrDefault(x => x.Id == input.TaskId);
            bool isSuccess = false;
            if (taskItem is not null)
            {
                taskItem.Status = LRTaskStatus.Completed.ToString();
                var lrTransactionDbSet = _unitOfWork.GetDbSet<LRTransaction>();
                var transactionItem = lrTransactionDbSet.FirstOrDefault(x => x.Id == taskItem.TransactionId);
                if (transactionItem is not null)
                {
                    transactionItem.Status = input.IsApprove == true ? LRWorkflowStatus.Approved.ToString() : LRWorkflowStatus.Rejected.ToString();
                    transactionItem.Comment = input.Comment;
                }
                isSuccess = await _unitOfWork.SaveChangesAsync() > 0;
            }
            _logger.LogInformation($"WF instance {context.InstanceId} manager approval done. Result: {JsonSerializer.Serialize(input)} {input}");
            return new LRProcessApproveTransactionResponse(isSuccess, ["Approve Success"]);
        }
    }
    public record LRProcessApproveTransactionRequest(long TaskId, bool IsApprove, string Comment);
    public record LRProcessApproveTransactionResponse(bool IsSuccess, string[] Messages);
}
