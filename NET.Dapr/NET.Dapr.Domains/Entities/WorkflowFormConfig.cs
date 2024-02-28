using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    [Table("WORKFLOW_FORM_CONFIG")]
    public class WorkflowFormConfig:BaseEntity
    {
        [Column("WF_CODE")]
        [MaxLength(255)]
        public string WfCode { get; set; }
        [Column("FORM_ID")]
        public long FormId { get; set; }
        [Column("WF_STEP")]
        [MaxLength(255)]
        public string WfStep { get; set; }
    }
}
