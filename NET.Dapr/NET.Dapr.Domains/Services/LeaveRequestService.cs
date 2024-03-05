using Dapr.Client;
using NET.Dapr.Domains.Entities;
using NET.Dapr.Domains.Infra;
using NET.Dapr.Domains.Models.ServiceModels;
using NET.Dapr.Domains.Workflows.LeaveRequest;
using static NET.Dapr.Domains.Workflows.Consts;

namespace NET.Dapr.Domains.Services
{
    public interface ILeaveRequestService
    {
        Task<StartWorkflowResponse> LRSubmit(LRBaseModel postModel);
    }
    public class LeaveRequestService (IUnitOfWork unitOfWork) : ILeaveRequestService
    {
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        const string workflowComponent = "dapr";
        const string workflowName = "LeaveRequestWorkflow";

        Dictionary<string, string> workflowOptionDics = new();

        readonly DaprClient daprClient = new DaprClientBuilder().Build();

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
    }
    public class LRSearchModel()
    {
        public string WfInstanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string DivisionCode { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
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
        public string WorkflowInstanceId { get; set; }

    }
}
