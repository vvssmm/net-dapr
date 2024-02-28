using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    [Table("EMAIL_HISTORIES")]
    public class EmailHistories: BaseEntity
    {
        [Column("SUBJECT")]
        [MaxLength(500)]
        public string Subject { get; set; }
        [Column("CONTENT")]
        [MaxLength(1000)]
        public string Content { get; set; }
        [Column("TO_EMAIL")]
        [MaxLength(255)]
        public string ToEmail { get; set; }
        [Column("CC_EMAIL")]
        [MaxLength(255)]
        public string CcEmail { get; set; }
        [Column("STATUS")]
        public int Status { get; set; }
    }
}
