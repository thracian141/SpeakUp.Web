using Microsoft.AspNetCore.Mvc;

namespace SpeakUp.Web.Controllers {
    [Route("index")]
    public class IndexController : Controller {
        [HttpGet("home")]
        public async Task<IActionResult> Index() {
            return new JsonResult(new { message = "Hello from the backend!" });
        }
    }
}
