using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using System.Threading.Tasks;

namespace NET.Dapr.Domains.Services
{
    public interface ITaskService
    {
        Task TaskApproval(WorkflowApprovalApiModel taskApprovalPayload);
    }
    public class TasksService(IUnitOfWork unitOfWork) : ITaskService
    {
        const string workflowComponent = "dapr";
        const string workflowName = "LeaveRequestWorkflow";
        const string workflowApproveEventName = "ManagerApproval";

        readonly IUnitOfWork _unitOfWork = unitOfWork;
        Dictionary<string, string> workflowOptionDics = [];

        readonly DaprClient daprClient = new DaprClientBuilder().Build();
        public async Task TaskApproval(WorkflowApprovalApiModel taskApprovalPayload)
        {
            var transactionDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var transactionItem = await transactionDbSet.FirstOrDefaultAsync(t=>t.Id == taskApprovalPayload.TransactionId);
            if(transactionItem is not null)
            {
                var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
                var taskItem = await taskDbSet.FirstOrDefaultAsync(t=>t.TransactionId == taskApprovalPayload.TransactionId);
                ManagerApprovalResult postModel = new()
                {
                    TaskId = taskItem.Id,
                    IsApproved = taskApprovalPayload.IsApproved,
                    Comment = taskApprovalPayload.Comment
                };
                 await daprClient.RaiseWorkflowEventAsync(
                                instanceId: transactionItem.WfInstanceId,
                                workflowComponent, 
                                eventName:workflowApproveEventName,
                                eventData: postModel);
            }
            else
            {
                throw new Exception($"Transaction {taskApprovalPayload.TransactionId} not found");
            }
        }
    }
}
