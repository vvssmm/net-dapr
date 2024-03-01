using Dapr.Workflow;
using Microsoft.Extensions.Logging;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
   
    public class LRGetApproverActivity(ILoggerFactory loggerFactory) : WorkflowActivity<GetApproverRequest, GetApproverResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LRGetApproverActivity>();

        public override Task<GetApproverResponse> RunAsync(WorkflowActivityContext context, GetApproverRequest input)
        {
            _logger.LogDebug("Access to Get Approver Activity");

            _logger.LogInformation($"Workflow instance {context.InstanceId} is getting approver");
            var getApproverResult = new GetApproverResponse("FES", "Võ Sĩ Mến","menvs@unit.com.vn");
            _logger.LogInformation($"Workflow instance {context.InstanceId} get approver done. Result: {getApproverResult}");
            return Task.FromResult(getApproverResult);
        }
    }
    public record GetApproverRequest(string DivisionCode);
    public record GetApproverResponse(string Code, string Name, string Email);
}
