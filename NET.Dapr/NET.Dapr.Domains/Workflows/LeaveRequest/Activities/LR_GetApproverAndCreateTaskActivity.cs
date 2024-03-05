using Dapr.Workflow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using static NET.Dapr.Domains.Workflows.Consts;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
   
    public class LR_GetApproverAndCreateTaskActivity(ILoggerFactory loggerFactory, IUnitOfWork unitOfWork) : WorkflowActivity<GetApproverAndCreateTaskRequest, GetApproverAndCreateTaskResponse>
    {
        readonly ILogger _logger = loggerFactory.CreateLogger<LR_GetApproverAndCreateTaskActivity>();
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        public override async Task<GetApproverAndCreateTaskResponse> RunAsync(WorkflowActivityContext context, GetApproverAndCreateTaskRequest input)
        {
            _logger.LogInformation($"WF {context.InstanceId} Access to Get Approver Activity");
            _logger.LogInformation($"WF {context.InstanceId} is getting approver");
            var approvalConfigDbSet = _unitOfWork.GetDbSet<ApproverConfig>();
            var approverConfig = await approvalConfigDbSet.FirstOrDefaultAsync(x => x.DivisionCode == input.DivisionCode);
            GetApproverAndCreateTaskResponse getApproverResult = default;
            if (approverConfig is not null)
            {
                var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
                var leaveRequest = await leaveRequestDbSet.FirstOrDefaultAsync(x => x.Id == input.TransactionId);
                leaveRequest.Status = LRWorkflowStatus.WaitingForApproval.ToString();
                leaveRequest.Approver = approverConfig.Code;

                var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
                var task = new LRTasks()
                {
                    TaskName = $"[{context.InstanceId}] Review and Aprroval for Leave Request ID {input.TransactionId} {input.EmployeeCode} - {input.EmployeeName}",
                    Status = LRTaskStatus.New.ToString(),
                    Assignee = approverConfig.Code,
                    AssigneeEmail = approverConfig.Email,
                    TransactionId = input.TransactionId
                };
                var taskEntity= await taskDbSet.AddAsync(task);

                await _unitOfWork.SaveChangesAsync();
                getApproverResult = new GetApproverAndCreateTaskResponse(approverConfig.DivisionCode, approverConfig.Name,approverConfig.Email, taskEntity.Entity.Id);
            }
            _logger.LogInformation($"WF {context.InstanceId} get approver done. Result: {getApproverResult}");
            return getApproverResult;
        }
    }
    public record GetApproverAndCreateTaskRequest(
        string DivisionCode,
        string EmployeeCode, string EmployeeName,DateTime? DateOffFrom,DateTime?DateOffTo, long TransactionId);
    public record GetApproverAndCreateTaskResponse(string Code, string Name, string Email, long TaskId);
}
