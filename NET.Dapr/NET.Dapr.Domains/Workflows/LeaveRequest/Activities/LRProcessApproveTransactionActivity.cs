using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public class LRProcessApproveTransactionActivity(ILoggerFactory loggerFactory) : WorkflowActivity<LRProcessApproveTransactionRequest, LRProcessApproveTransactionResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LRProcessApproveTransactionActivity>();

        public override Task<LRProcessApproveTransactionResponse> RunAsync(WorkflowActivityContext context,LRProcessApproveTransactionRequest input)
        {
            _logger.LogDebug("Access to LRProcessApproveTransactionActivity");
            _logger.LogInformation($"Workflow instance {context.InstanceId} manager approval done. Result: {JsonSerializer.Serialize(input)} {input}");

            return new Task<LRProcessApproveTransactionResponse>(() => new LRProcessApproveTransactionResponse(true, ["Approve Success"]));
        }
    }
    public record LRProcessApproveTransactionRequest(DateTime? ApproveTime, bool IsApprove, string Comment);
    public record LRProcessApproveTransactionResponse(bool IsSuccess, string[] Messages);
}
