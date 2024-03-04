namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models.ApiModels
{
    public class ApiResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public List<string> StackTraces { get; set; }
    }
}
