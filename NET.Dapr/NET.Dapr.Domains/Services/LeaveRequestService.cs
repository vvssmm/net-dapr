using AutoMapper;
using Dapr.Client;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Models.ServiceModels;
using NET.Dapr.Domains.Workflows.LeaveRequest;
using System.Linq.Expressions;
using static NET.Dapr.Domains.Consts;

namespace NET.Dapr.Domains.Services
{
    public interface ILeaveRequestService
    {
        Task<StartWorkflowResponse> LRSubmit(LRBaseModel postModel);
        Task<(List<LRDataModel>, int)> Search(LRSearchModel searchModel);
        Task<LRDataModel> GetById(long id);
    }
    public class LeaveRequestService(IUnitOfWork unitOfWork,IMapper mapper) : ILeaveRequestService
    {
        Dictionary<string, string> workflowOptionDics = [];

        readonly DaprClient daprClient = new DaprClientBuilder(){}.Build();
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly IMapper _mapper = mapper;
        public async Task<StartWorkflowResponse> LRSubmit(LRBaseModel postModel)
        {
            var lrEntity = _mapper.Map<LRTransaction>(postModel);
            string workflowInstaceId = $"{postModel.EmployeeCode}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            lrEntity.WfInstanceId = workflowInstaceId;
            lrEntity.Status = LRWorkflowStatus.GettingApprover.ToString();
             var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var responseEntity = await leaveRequestDbSet.AddAsync(lrEntity);
            int impactRows = await _unitOfWork.SaveChangesAsync();
            if (impactRows > 0)
            {
                var workflowStartModel = _mapper.Map<LRStartWorkflowPayload>(responseEntity.Entity);
                workflowStartModel.TransactionId = responseEntity.Entity.Id;
                var res = await daprClient.StartWorkflowAsync(
                                WfConfig.WorkflowComponet, WfConfig.LeaveRequest.WorkflowName,
                                instanceId: workflowInstaceId,
                                workflowOptions: workflowOptionDics,
                                input: workflowStartModel);

                return res;
            }

            return null;
        }
        public async Task<(List<LRDataModel>, int)> Search(LRSearchModel searchModel)
        {
            List<LRDataModel> result = [];

            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            Expression<Func<LRTransaction, bool>> whereExpression = t => true;
            if (!string.IsNullOrEmpty(searchModel.WfInstanceId))
            {
                whereExpression = whereExpression.And(l=>l.WfInstanceId == searchModel.WfInstanceId);
            }
            if (!string.IsNullOrEmpty(searchModel.EmployeeCode))
            {
                whereExpression = whereExpression.And(l => l.EmployeeCode == searchModel.EmployeeCode);
            }
            if (!string.IsNullOrEmpty(searchModel.DivisionCode))
            {
                whereExpression = whereExpression.And(l => l.DivisionCode == searchModel.DivisionCode);
            }

            var query = leaveRequestDbSet.Where(whereExpression).OrderByDescending(t => t.CreatedDate).AsNoTracking();
            int totalCount = await query.CountAsync();
            var resultEntities = await query.Skip((searchModel.PageIndex - 1) * searchModel.PageSize)
                            .Take(searchModel.PageSize).ToListAsync();
            var returnData = _mapper.Map<List<LRDataModel>>(resultEntities);

            return (returnData, totalCount);
        }
        public async Task<LRDataModel> GetById(long id)
        {
            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var entity = await leaveRequestDbSet.FirstOrDefaultAsync(t=>t.Id == id);

            if(entity is not null)
            {
                return _mapper.Map<LRDataModel>(entity);
            }
            return null;
        }
    }
    public class LRSearchModel()
    {
        public string WfInstanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string DivisionCode { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
    public class LRBaseModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public DateTime? DateOffFrom { get; set; }
        public DateTime? DateOffTo { get; set; }
        public string PeriodDateOffFrom { get; set; }
        public string PeriodDateOffTo { get; set; }
        public string LeaveRequestType { get; set; }
        public string LeaveRequestDateType { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }
    public class LRDataModel : BaseModel
    {
        public string WfInstanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public DateTime? DateOffFrom { get; set; }
        public DateTime? DateOffTo { get; set; }
        public string PeriodDateOffFrom { get; set; }
        public string PeriodDateOffTo { get; set; }
        public string LeaveRequestType { get; set; }
        public string LeaveRequestDateType { get; set; }
        public string Reason { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Approver { get; set; }

    }
}
