using Microsoft.AspNetCore.Mvc;
using NET.Dapr.Domains.Models.ApiModels;
using NET.Dapr.Domains.Services;
using NET.Dapr.Domains.Workflows.LeaveRequest.Models;

namespace NET.Dapr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController(ITaskService taskService): ControllerBase
    {
        readonly ITaskService _taskService = taskService;
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] WorkflowApprovalApiModel payloadSubmit)
        {
            var apiModel = new ApiResultModel<string>();
             await _taskService.TaskApproval(payloadSubmit);
            apiModel.Success = true;
            return Ok(apiModel);
        }
    }
}
