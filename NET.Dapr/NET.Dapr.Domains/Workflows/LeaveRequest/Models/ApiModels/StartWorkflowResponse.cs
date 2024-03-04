namespace NET.Dapr.Domains.Workflows.LeaveRequest.Models.ApiModels
{
    public class StartWorkflowResponseApiModel
    {
        public string WorkflowInstanceId { get; set; }
        public List<string> Messages { get; set; }
    }
}
