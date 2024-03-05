using Dapr.Workflow;
using Microsoft.Extensions.Logging;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using static NET.Dapr.Domains.Workflows.Consts;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    public class LR_ReminderApprovalTaskActivity(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork) : WorkflowActivity<ReminderRequest, bool>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LR_ReminderApprovalTaskActivity>();
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        public override Task<bool> RunAsync(WorkflowActivityContext context, ReminderRequest input)
        {
            _logger.LogInformation($"WF instance {context.InstanceId}. Access to Reminder approval Activity. Repeatation {input.Repeatations} every {input.Minutes} minutes for Task {input.TaskId}");
            var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
            int index = 0;
            while (index < input.Repeatations)
            {
                Task.Delay(TimeSpan.FromMinutes(input.Minutes)).Wait();
                var taskItem = taskDbSet.FirstOrDefault(t=>t.Id==input.TaskId);
                if (taskItem is not null && taskItem.Status == LRTaskStatus.New.ToString())
                {
                    index++;
                    var emailHistoryDbSet = _unitOfWork.GetDbSet<EmailHistories>();
                    var emailHistory = new EmailHistories()
                    {
                        Subject = $"[{context.InstanceId}][Reminder {index}] {taskItem.TaskName}",
                        Content = $"Dear {taskItem.Assignee}, <br/><br/> Please review and approve this task: {taskItem.TaskName}",
                        ToEmail = taskItem.AssigneeEmail,
                        Status = (int)LREmailStatus.Sent,
                        TransactionID = taskItem.TransactionId
                    };

                     emailHistoryDbSet.Add(emailHistory);
                    int impactRows= _unitOfWork.SaveChanges();
                    _logger.LogInformation($"WF instance {context.InstanceId} Reminder at {index} time. Success {impactRows > 0}");
                }
                else
                {
                    _logger.LogInformation($"WF instance {context.InstanceId} End Reminder at {index} time cause has task has final status");
                    break;
                }
            }
            return Task.FromResult(true);
        }
    }
    public record ReminderRequest(long TaskId, int Repeatations, int Minutes);
}
