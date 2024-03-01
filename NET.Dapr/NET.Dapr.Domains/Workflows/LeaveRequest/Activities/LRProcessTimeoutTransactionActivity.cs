using Dapr.Workflow;
using Microsoft.Extensions.Logging;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public class LRProcessTimeoutTransactionActivity(ILoggerFactory loggerFactory) : WorkflowActivity<LRTimeoutRequest, LRTimeoutResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LRProcessTimeoutTransactionActivity>();
        public override Task<LRTimeoutResponse> RunAsync(WorkflowActivityContext context, LRTimeoutRequest input)
        {
            _logger.LogDebug("Access to LRProcessTimeoutTransactionActivity");
            _logger.LogInformation($"Workflow instance {context.InstanceId} auto terminate because approver not respond after 10 minutes");
            throw new NotImplementedException();
        }
    }
    public record LRTimeoutRequest();
    public record LRTimeoutResponse();
}
