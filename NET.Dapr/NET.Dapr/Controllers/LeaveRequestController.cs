using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using NET.Dapr.Domains.Models.ApiModels;
using NET.Dapr.Domains.Services;

namespace NET.Dapr.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LeaveRequestController(ILeaveRequestService leaveRequestService) : ControllerBase
    {
        readonly ILeaveRequestService _leaveRequestService = leaveRequestService;
        [HttpPost("Submit")]
        public async Task<IActionResult> SubmitForm([FromBody] LRBaseModel payloadSubmit)
        {
            var apiModel = new ApiResultModel<StartWorkflowResponse>();
            var response = await _leaveRequestService.LRSubmit(payloadSubmit);
            apiModel.Data = response;
            apiModel.Success = true;
            return Ok(apiModel);
        }
    }
}
