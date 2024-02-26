using Dapr.Workflow;

namespace NET.Dapr.Domains.Workflows.LeaveRequest.Activities
{
    internal record GetApprovalRequest(string EmplpoyeeCode, string DivisionCode);
    internal record GetApprovalResponse(string ApprovalCode, string ApprovalEmail);
    internal class LRGetApprovalActivity : WorkflowActivity<GetApprovalRequest, GetApprovalResponse>
    {
        public override Task<GetApprovalResponse> RunAsync(WorkflowActivityContext context, GetApprovalRequest input)
        {
            throw new NotImplementedException();
        }
    }
}
