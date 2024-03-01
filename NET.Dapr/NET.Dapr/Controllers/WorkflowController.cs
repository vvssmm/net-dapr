using Microsoft.AspNetCore.Mvc;

namespace NET.Dapr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkflowController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
