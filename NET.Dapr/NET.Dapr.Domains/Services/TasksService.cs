using AutoMapper;
using Dapr.Client;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Models.ServiceModels;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using System.Linq.Expressions;
using static NET.Dapr.Domains.Consts;

namespace NET.Dapr.Domains.Services
{
    public interface ITaskService
    {
        Task TaskApproval(WorkflowApprovalApiModel taskApprovalPayload, CancellationToken cancellationToken = default);
        Task<(List<LRTaskApiModel>, int)> Search(LRTaskSearchModel searchModel, CancellationToken cancellationToken = default);
        Task<LRTaskApiModel> GetById(long id, CancellationToken cancellationToken = default);
    }
    public class TasksService(IUnitOfWork unitOfWork, IMapper mapper) : ITaskService
    {
        Dictionary<string, string> workflowOptionDics = [];

        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly DaprClient daprClient = new DaprClientBuilder().Build();
        readonly IMapper _mapper = mapper;
        public async Task TaskApproval(WorkflowApprovalApiModel taskApprovalPayload,CancellationToken cancellationToken = default)
        {
            var transactionDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var transactionItem = await transactionDbSet.FirstOrDefaultAsync(t=>t.Id == taskApprovalPayload.TransactionId,cancellationToken);
            if (transactionItem is not null)
            {
                var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
                var taskItem = await taskDbSet.FirstOrDefaultAsync(t=>t.TransactionId == taskApprovalPayload.TransactionId,cancellationToken);

                ManagerApprovalResult postModel = new()
                {
                    TaskId = taskItem.Id,
                    IsApproved = taskApprovalPayload.IsApproved,
                    Comment = taskApprovalPayload.Comment
                };

                await daprClient.RaiseWorkflowEventAsync(
                               instanceId: transactionItem.WfInstanceId,
                               WfConfig.WorkflowComponet,
                               eventName: WfConfig.LeaveRequest.WorkflowApprovalEventName,
                               eventData: postModel, cancellationToken);
            }
            else
            {
                throw new Exception($"Transaction {taskApprovalPayload.TransactionId} not found");
            }
        }
        public async Task<(List<LRTaskApiModel>, int)> Search(LRTaskSearchModel searchModel, CancellationToken cancellationToken = default)
        {
            List<LRTaskApiModel> result = [];

            var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();
            var query = taskDbSet.AsQueryable();
            Expression<Func<LRTasks, bool>> whereExpression = t => true;
            if (searchModel.TransactionId.HasValue)
            {
                whereExpression = whereExpression.And(t => t.TransactionId == searchModel.TransactionId);
            }
            if (!string.IsNullOrEmpty(searchModel.Assignee))
            {
                whereExpression = whereExpression.And(t => t.Assignee == searchModel.Assignee);
            }

            var returnEntityQueryable = taskDbSet.Where(whereExpression).OrderByDescending(t => t.CreatedDate);
            int totalCount = await returnEntityQueryable.CountAsync(cancellationToken);
            var taskEntities = await returnEntityQueryable
                            .Skip((searchModel.PageIndex - 1) * searchModel.PageSize)
                            .Take(searchModel.PageSize).ToListAsync(cancellationToken);
            //var taskData = _mapper.Map<List<LRTaskApiModel>>(taskEntities);
            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var returnData = taskEntities.Join(leaveRequestDbSet, task => task.TransactionId, lr => lr.Id, (task, lr) => new { task, lr }).Select(obj=>
            {
                var taskApiModel = _mapper.Map<LRTaskApiModel>(obj.task);
                taskApiModel.EmployeeCode = obj.lr.EmployeeCode;
                taskApiModel.EmployeeName = obj.lr.EmployeeName;
                taskApiModel.DivisionCode = obj.lr.DivisionCode;
                return taskApiModel;
            }).ToList();
            return (returnData, totalCount);
        }
        public async Task<LRTaskApiModel> GetById(long id, CancellationToken cancellationToken = default)
        {
            var taskDbSet = _unitOfWork.GetDbSet<LRTasks>();

            var entity = await taskDbSet.FirstOrDefaultAsync(t=>t.Id == id,cancellationToken);

            if (entity is not null)
            {
                var taskData =  mapper.Map<LRTaskApiModel>(entity);
                var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
                var lrItem = await leaveRequestDbSet.FirstOrDefaultAsync(l=>l.Id == taskData.TransactionId,cancellationToken);
                taskData.EmployeeCode = lrItem?.EmployeeCode;
                taskData.EmployeeName = lrItem?.EmployeeName;
                taskData.DivisionCode = lrItem?.DivisionCode;
            }
            return null;
        }
    }
    public class LRTaskSearchModel
    {
        public long? TransactionId { get; set; }
        public string? Assignee { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
    }
    public class LRTaskApiModel:BaseModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public string TaskName { get; set; }
        public string Assignee { get; set; }
        public string AssigneeEmail { get; set; }
        public string Status { get; set; }
        public long? TransactionId { get; set; }
    }
}
