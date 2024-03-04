namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models
{
    public class WorkflowApprovalApiModel
    {
        public string WfInstanceId { get; set; }
        public ManagerApprovalResult ApprovalResult { get; set; }
    }
    public class ManagerApprovalResult
    {
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
    }
}
