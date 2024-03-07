using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    [Table("TASKS")]
    public class LRTasks:BaseEntity
    {
        [Column("TASK_NAME")]
        [MaxLength(500)]
        public string TaskName { get; set; }
        [Column("ASSIGNEE")]
        [MaxLength(255)]
        public string Assignee { get; set; }
        [Column("ASSIGNEE_EMAIL")]
        [MaxLength(255)]
        public string AssigneeEmail { get; set; }
        [Column("STATUS")]
        [MaxLength(100)]
        public string Status { get; set; }
        [Column("TRANSACTION_ID")]
        public long? TransactionId { get; set; }
    }
}
