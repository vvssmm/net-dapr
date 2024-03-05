namespace NET.Dapr.Domains.Models.ApiModels
{
    public class ApiResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Messages { get; set; } = [];
        public List<string> Errors { get; set; } = [];
        public List<string> StackTraces { get; set; } = [];
    }
    public class ApiSearchResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }
        public List<string> Messages { get; set; } = [];
        public List<string> Errors { get; set; } = [];
        public List<string> StackTraces { get; set; } = [];
    }
}
