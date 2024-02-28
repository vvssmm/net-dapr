using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    [Table("APPROVER_CONFIG")]
    public class ApproverConfig:BaseEntity
    {
        [Column("DIVISION_CODE")]
        [MaxLength(255)]
        public string DivisionCode { get; set; }
        [Column("NAME")]
        [MaxLength(255)]
        public string Name { get; set; }
        [Column("EMAIL")]
        [MaxLength(255)]
        public string Email { get; set; }
        [Column("CODE")]
        [MaxLength(255)]
        public string Code { get; set; }
    }
}
