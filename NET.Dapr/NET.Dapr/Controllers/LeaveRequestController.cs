using Microsoft.AspNetCore.Mvc;
using NET.Dapr.Domains.Workflows.LeaveRequest;

namespace NET.Dapr.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class LeaveRequestController : ControllerBase
    {
        [HttpPost("Submit")]
        public IActionResult SubmitForm([FromBody] LRSubmitPayload payloadSubmit)
        {
            // return View();

            return Ok();
        }
    }
}
