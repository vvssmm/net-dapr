namespace NET.Dapr.Domains.Models.ServiceModels
{
    public class BaseModel
    {
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
