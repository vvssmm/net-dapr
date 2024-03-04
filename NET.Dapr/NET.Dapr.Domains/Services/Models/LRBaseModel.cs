namespace NET.Dapr.Domains.Services.Models
{
    public class LRBaseModel
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DivisionCode { get; set; }
        public DateTime? DateOffFrom { get; set; }
        public DateTime? DateOffTo { get; set; }
        public string LeaveRequestType { get; set; }
        public string Reason { get; set; }
    }
}
