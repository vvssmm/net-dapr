namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models
{
    public record LRSubmitPayload
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public string PositionCode { get; set; }
        public DateTime? DateOffFrom { get; set; }
        public DateTime? DateOffTo { get; set; }
        public string Reason { get; set; }
    }
}
