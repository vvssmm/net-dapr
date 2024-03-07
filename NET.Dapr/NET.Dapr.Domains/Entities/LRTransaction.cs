using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    [Table("LR_TRANSACTION")]
    public class LRTransaction:BaseEntity
    {
        [Column("WF_INSTANCE_ID")]
        [MaxLength(255)]
        public string WfInstanceId { get; set; }
        [Column("EMPLOYEE_CODE")]
        [MaxLength(255)]
        public string EmployeeCode { get; set; }
        [Column("EMPLOYEE_NAME")]
        [MaxLength(255)]
        public string EmployeeName { get; set; }
        [Column("DIVISION_CODE")]
        [MaxLength(255)]
        public string DivisionCode { get; set; }
        [Column("STATUS")]
        public string Status { get; set; }
        [Column("REASON")]
        [MaxLength(1000)]
        public string Reason { get; set; }
        [Column("DATE_OFF_FROM")]
        public DateTime? DateOffFrom { get; set; }
        [Column("DATE_OFF_TO")]
        public DateTime? DateOffTo { get; set; }
        [Column("APPROVER")]
        public string Approver { get; set; }
        [Column("CC")]
        [MaxLength(255)]
        public string Cc { get; set; }
        [Column("BCC")]
        [MaxLength(255)]
        public string Bcc { get; set; }
        [Column("LEAVE_REQUEST_TYPE")]
        [MaxLength(100)]
        public string LeaveRequestType { get; set; }
        [Column("PERIOD_DATE_OFF_FROM")]
        [MaxLength(255)]
        public string PeriodDateOffFrom { get; set; }
        [Column("PERIOD_DATE_OFF_TO")]
        [MaxLength(255)]
        public string PeriodDateOffTo { get; set; }
        [Column("LEAVE_REQUEST_DATE_TYPE")]
        [MaxLength(255)]
        public string LeaveRequestDateType { get; set; }
        [Column("COMMENT")]
        [MaxLength(500)]
        public string Comment { get; set; }
        [Column("DATE_OFF")]
        public DateTime? DateOff { get; set; }
        [Column("PERIOD_DATE_OFF")]
        [MaxLength(255)]
        public string PeriodDateOff { get; set; }
    }
}
