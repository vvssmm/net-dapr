using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public record LRSendEmailNotifyRequest(string EmailCode, object Data);
    public record LRSendEmailNotifyResponse(bool IsSuccess, List<string> Messages);
    public class LRSendEmailNotifyActivity(ILoggerFactory loggerFactory) : WorkflowActivity<LRSendEmailNotifyRequest, LRSendEmailNotifyResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LRSendEmailNotifyActivity>();
        public override Task<LRSendEmailNotifyResponse> RunAsync(WorkflowActivityContext context, LRSendEmailNotifyRequest input)
        {
            _logger.LogDebug("Access to LRSendEmailNotifyActivity");
            _logger.LogInformation($"Workflow instance {context.InstanceId} send email code {input.EmailCode} {JsonSerializer.Serialize(input.Data)}");
            return Task.FromResult(new LRSendEmailNotifyResponse(true, ["Send Email Success"]));
        }
    }
}
