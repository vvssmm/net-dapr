namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models
{
    internal class ManagerApprovalResult
    {
        public bool IsApproved { get; set; }
        public string Messages { get; set; }
    }
}
