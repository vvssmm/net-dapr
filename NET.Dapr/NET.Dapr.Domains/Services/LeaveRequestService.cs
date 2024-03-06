using Dapr.Client;
using Microsoft.EntityFrameworkCore;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Models.ServiceModels;
using NET.Dapr.Domains.Workflows.LeaveRequest;

namespace NET.Dapr.Domains.Services
{
    public interface ILeaveRequestService
    {
        Task<StartWorkflowResponse> LRSubmit(LRBaseModel postModel);
        Task<(List<LRDataModel>, int)> Search(LRSearchModel searchModel);
        Task<LRDataModel> GetById(long id);
    }
    public class LeaveRequestService(IUnitOfWork unitOfWork) : ILeaveRequestService
    {
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        const string workflowComponent = "dapr";
        const string workflowName = "LeaveRequestWorkflow";

        Dictionary<string, string> workflowOptionDics = new();

        readonly DaprClient daprClient = new DaprClientBuilder()
        {
            grpcPort = 50001
        }.Build();

        public async Task<StartWorkflowResponse> LRSubmit(LRBaseModel postModel)
        {
            var lrEntity = new LRTransaction();
            string workflowInstaceId = $"{postModel.EmployeeCode}-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            lrEntity.WfInstanceId = workflowInstaceId;
            lrEntity.EmployeeCode = postModel.EmployeeCode;
            lrEntity.EmployeeName = postModel.EmployeeName;
            lrEntity.DivisionCode = postModel.DivisionCode;
            lrEntity.DateOffFrom = postModel.DateOffFrom;
            lrEntity.DateOffTo = postModel.DateOffTo;
            lrEntity.Reason = postModel.Reason;
            lrEntity.Cc = postModel.Cc;
            lrEntity.Bcc = postModel.Bcc;
            lrEntity.LeaveRequestType = postModel.LeaveRequestType;
            lrEntity.LeaveRequestDateType = postModel.LeaveRequestDateType;
            lrEntity.PeriodDateOffFrom = postModel.PeriodDateOffFrom;
            lrEntity.PeriodDateOffTo = postModel.PeriodDateOffTo;
            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();
            var responseEntity = await leaveRequestDbSet.AddAsync(lrEntity);
            int impactRows = await _unitOfWork.SaveChangesAsync();
            if (impactRows > 0)
            {
                var workflowStartModel = new LRStartWorkflowPayload(
                    responseEntity.Entity.Id,
                    postModel.EmployeeCode,
                    postModel.EmployeeName,
                    postModel.DivisionCode,
                    postModel.DateOffFrom,
                    postModel.DateOffTo,
                    postModel.Reason,
                    postModel.Cc,
                    postModel.Bcc);

                var res = await daprClient.StartWorkflowAsync(
                                workflowComponent, workflowName,
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
            var query = leaveRequestDbSet.AsQueryable();
            if (!string.IsNullOrEmpty(searchModel.WfInstanceId))
            {
                query = query.Where(t => t.WfInstanceId == searchModel.WfInstanceId);
            }
            if (!string.IsNullOrEmpty(searchModel.EmployeeCode))
            {
                query = query.Where(t => t.EmployeeCode == searchModel.EmployeeCode);
            }
            if (!string.IsNullOrEmpty(searchModel.DivisionCode))
            {
                query = query.Where(t => t.DivisionCode == searchModel.DivisionCode);
            }
            int totalCount = await query.CountAsync();
            var returnEntityLs = await query.OrderByDescending(t => t.CreatedDate)
                            .Skip((searchModel.PageIndex - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            var returnData = returnEntityLs.Select(t => new LRDataModel()
            {
                Id = t.Id,
                WfInstanceId = t.WfInstanceId,
                EmployeeCode = t.EmployeeCode,
                EmployeeName = t.EmployeeName,
                DivisionCode = t.DivisionCode,
                DateOffFrom = t.DateOffFrom,
                DateOffTo = t.DateOffTo,
                Reason = t.Reason,
                Comment = t.Comment,
                Status = t.Status,
                Cc = t.Cc,
                Bcc = t.Bcc,
                CreatedDate = t.CreatedDate,
                UpdatedDate = t.UpdatedDate,
                LeaveRequestType = t.LeaveRequestType,
                LeaveRequestDateType = t.LeaveRequestDateType,
                PeriodDateOffFrom = t.PeriodDateOffFrom,
                PeriodDateOffTo = t.PeriodDateOffTo,
                Approver = t.Approver
            }).ToList();

            return (returnData, totalCount);
        }
        public async Task<LRDataModel> GetById(long id)
        {
            var leaveRequestDbSet = _unitOfWork.GetDbSet<LRTransaction>();

            var entity = await leaveRequestDbSet.FirstOrDefaultAsync(t=>t.Id == id);

            if(entity is not null)
            {
                return new LRDataModel()
                {
                    Id = entity.Id,
                    WfInstanceId = entity.WfInstanceId,
                    EmployeeCode = entity.EmployeeCode,
                    EmployeeName = entity.EmployeeName,
                    DivisionCode = entity.DivisionCode,
                    DateOffFrom = entity.DateOffFrom,
                    DateOffTo = entity.DateOffTo,
                    Reason = entity.Reason,
                    Comment = entity.Comment,
                    Status = entity.Status,
                    Cc = entity.Cc,
                    Bcc = entity.Bcc,
                    CreatedDate = entity.CreatedDate,
                    UpdatedDate = entity.UpdatedDate,
                    LeaveRequestType = entity.LeaveRequestType,
                    LeaveRequestDateType = entity.LeaveRequestDateType,
                    PeriodDateOffFrom = entity.PeriodDateOffFrom,
                    PeriodDateOffTo = entity.PeriodDateOffTo,
                    Approver = entity.Approver
                };
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
