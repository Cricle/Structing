using Microsoft.AspNetCore.Mvc;

namespace Structing.HotReload.School.Controllers
{
    [ApiController]
    [Route("school")]
    public class SchoolController:ControllerBase
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok(123);
        }
    }
}
