namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models
{
    public class WorkflowApprovalApiModel
    {
        public long TransactionId { get; set; }
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
    }
    public class ManagerApprovalResult
    {
        public long TaskId { get; set; }
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
    }
}
