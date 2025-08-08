using Microsoft.AspNetCore.Mvc;

namespace DefenSys.Server.Controllers
{

    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            var statusObject = new { Status = "Penetration Test API is running!", Timestamp = DateTime.UtcNow };
            return Ok(statusObject);
        }
    }
}