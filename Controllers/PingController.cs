using Microsoft.AspNetCore.Mvc;

namespace AbhayMVCapp.Controllers
{
    [ApiController] // Marks this as Web API Controller
    [Route("api/[controller]")] // URL Will be /api/ping suffi "controller will be removed automaticlly
    public class PingController : ControllerBase
    {
        [HttpGet] //get request
        public IActionResult GetPing()
        {
            return Ok(new { message = "pong" }); //Ok(...) sends a 200 status with JSON
        }
    }
}
