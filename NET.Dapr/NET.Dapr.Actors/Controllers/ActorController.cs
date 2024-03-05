using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;
using NET.Dapr.Domains.Actors;
using NET.Dapr.Domains.Models.ApiModels;

namespace NET.Dapr.Actors.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register()
        {
            var retResult = new ApiResultModel<string>();
            var actorId = new ActorId("acb");
            var proxy = ActorProxy.Create<ILRReminderApprovalTaskActor>(actorId, "LRReminderApprovalTaskActor");
            await proxy.RegisterReminderTask(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
            // await proxy.UnregisterReminderTask();
            retResult.Success = true;
            return Ok(retResult);
        }
    }
}
