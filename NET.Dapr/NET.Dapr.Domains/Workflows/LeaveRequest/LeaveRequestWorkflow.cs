using Dapr.Workflow;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Dapr.Domains.Workflows.LeaveRequest
{
    public class LeaveRequestWorkflow : Workflow<LRSubmitPayload, LRSubmitResult>
    {
        public override Task<LRSubmitResult> RunAsync(WorkflowContext context, LRSubmitPayload input)
        {
            throw new NotImplementedException();
        }
    }
}
