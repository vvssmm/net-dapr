namespace NET.Dapr.Domains.Models.ServiceModels
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Messages { get; set; }
    }
}
