using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Models.ServiceModels;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using static Grpc.Core.Metadata;

namespace NET.Dapr.Domains.Services
{
    public interface ITaskService
    {
        Task TaskApproval(WorkflowApprovalApiModel taskApprovalPayload);
        Task<(List<LRTaskDataModel>, int)> Search(LRTaskSearchModel searchModel);
        Task<LRTaskDataModel> GetById(long id);
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
            if (transactionItem is not null)
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
                               eventName: workflowApproveEventName,
                               eventData: postModel);
            }
            else
            {
                throw new Exception($"Transaction {taskApprovalPayload.TransactionId} not found");
            }
        }
        public async Task<(List<LRTaskDataModel>, int)> Search(LRTaskSearchModel searchModel)
        {
            List<LRTaskDataModel> result = [];

            var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
            var query = taskDbSet.AsQueryable();
            if (searchModel.TransactionId.HasValue)
            {
                query = query.Where(t => t.TransactionId == searchModel.TransactionId);
            }
            if (!string.IsNullOrEmpty(searchModel.Assignee))
            {
                query = query.Where(t => t.Assignee == searchModel.Assignee);
            }
            int totalCount = await query.CountAsync();
            var returnEntityLs = await query.OrderByDescending(t => t.CreatedDate)
                            .Skip((searchModel.PageIndex - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            var returnData = returnEntityLs.Select(t => new LRTaskDataModel()
            {
                Id = t.Id,
                TransactionId = t.TransactionId,
                Assignee = t.Assignee,
                Status = t.Status,
                AssigneeEmail = t.AssigneeEmail,
                TaskName = t.TaskName,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate
            }).ToList();

            return (returnData, totalCount);
        }
        public async Task<LRTaskDataModel> GetById(long id)
        {
            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTasks>();

            var entity = await leaveRequestDbSet.FirstOrDefaultAsync(t=>t.Id == id);

            if (entity is not null)
            {
                return new LRTaskDataModel()
                {
                    Id = entity.Id,
                    TransactionId = entity.TransactionId,
                    Assignee = entity.Assignee,
                    Status = entity.Status,
                    AssigneeEmail = entity.AssigneeEmail,
                    TaskName = entity.TaskName,
                    CreatedDate = entity.CreatedDate,
                    UpdatedDate = entity.UpdatedDate
                };
            }
            return null;
        }
    }
    public class LRTaskSearchModel()
    {
        public long? TransactionId { get; set; }
        public string? Assignee { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
    public class LRTaskDataModel():BaseModel
    {
        public string TaskName { get; set; }
        public string Assignee { get; set; }
        public string AssigneeEmail { get; set; }
        public string Status { get; set; }
        public long? TransactionId { get; set; }
    }
}
