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
        [HttpPost("approval")]
        public async Task<IActionResult> Approve([FromBody] WorkflowApprovalApiModel payloadSubmit)
        {
            var apiModel = new ApiResultModel<string>();
             await _taskService.TaskApproval(payloadSubmit);
            apiModel.Success = true;
            return Ok(apiModel);
        }
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] LRTaskSearchModel payloadSubmit)
        {
            var apiModel = new ApiSearchResultModel<List<LRTaskDataModel>>();
            var response = await _taskService.Search(payloadSubmit);
            apiModel.Data = response.Item1;
            apiModel.TotalCount = response.Item2;
            apiModel.Success = true;
            return Ok(apiModel);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var apiModel = new ApiResultModel<LRTaskDataModel>();
            var response = await _taskService.GetById(id);
            apiModel.Data = response;
            apiModel.Success = true;
            return Ok(apiModel);
        }
    }
}
