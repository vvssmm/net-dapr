using System.ComponentModel.DataAnnotations.Schema;

namespace NET.Dapr.Domains.Entities
{
    public class BaseEntity
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime? UpdatedDate { get; set; }
    }
}
