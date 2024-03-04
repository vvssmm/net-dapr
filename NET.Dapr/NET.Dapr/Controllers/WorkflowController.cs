using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using NET.Dapr.Domains.Workflows.LeaveRequest;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models.ApiModels;
using System.Text.Json;

namespace NET.Dapr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkflowController (ILoggerFactory loggerFactory) : ControllerBase
    {
        const string workflowComponent = "dapr";
        const string workflowName = "LeaveRequestWorkflow";
        const string workflowApproveEventName = "ManagerApproval";

        Dictionary<string, string> workflowOptionDics = new();

        readonly DaprClient daprClient = new DaprClientBuilder().Build();
        readonly ILogger logger = loggerFactory.CreateLogger<WorkflowController>();

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] LRSubmitPayload payloadSubmit)
        {
            var returnResult = new ApiResultModel<StartWorkflowResponseApiModel>();
            logger.LogDebug($"Receive body {JsonSerializer.Serialize(payloadSubmit)}");
            string newInstanceId = Guid.NewGuid().ToString();
            logger.LogDebug($"InstanceId: {newInstanceId}");

            var requestStartWorlflow = await daprClient.StartWorkflowAsync(
                workflowComponent,
                workflowName,
                instanceId:newInstanceId, 
                workflowOptions:workflowOptionDics,
                input:payloadSubmit);

            returnResult.Success = true;
            returnResult.Data = new StartWorkflowResponseApiModel()
            {
                WorkflowInstanceId = requestStartWorlflow.InstanceId,
                Messages = ["Start workflow success"]
            };
            return Ok(returnResult);
        }
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] WorkflowApprovalApiModel payloadApprove)
        {
             await daprClient.RaiseWorkflowEventAsync(payloadApprove.WfInstanceId, workflowComponent, workflowApproveEventName, payloadApprove.ApprovalResult);
            return Ok(new ApiResultModel<string>() { Success = true });
        }
    }
}
