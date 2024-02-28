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
        public int Status { get; set; }
        [Column("REASON")]
        [MaxLength(1000)]
        public string Reason { get; set; }
        [Column("DATE_OFF_FORM")]
        public DateTime? DateOffForm { get; set; }
        [Column("DATE_OFF_TO")]
        public DateTime? DateOffTo { get; set; }
        [Column("APPROVER")]
        public string Approver { get; set; }
    }
}
