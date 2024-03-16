using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using static NET.Dapr.Domains.Consts;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public record LRSendEmailNotifyRequest(long TransactionId, string Subject, string Body, string To, string Cc, string Bcc);
    public record LRSendEmailNotifyResponse(bool IsSuccess, List<string> Messages);
    public class LR_SendEmailNotifyActivity(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork) : WorkflowActivity<LRSendEmailNotifyRequest, LRSendEmailNotifyResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LR_SendEmailNotifyActivity>();
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public override async Task<LRSendEmailNotifyResponse> RunAsync(WorkflowActivityContext context, LRSendEmailNotifyRequest input)
        {
            _logger.LogInformation($"WF Instance {context.InstanceId} access Send email notify Activity");
            var emailHistoryDbSet = _unitOfWork.GetDbSet<EmailHistories>();
            var emailHistory = new EmailHistories()
            {
                Subject = input.Subject,
                Content = input.Body,
                ToEmail = input.To,
                CcEmail = input.Cc,
                BccEmail = input.Bcc,
                Status = (int)LREmailStatus.Sent,
                TransactionID = input.TransactionId
            };
            await emailHistoryDbSet.AddAsync(emailHistory);
            int impactRows=await _unitOfWork.SaveChangesAsync();
            bool isSuccess = impactRows > 0;
            return new LRSendEmailNotifyResponse(isSuccess, [$"Send Email Success {isSuccess}"]);
        }
    }
}
