using Microsoft.AspNetCore.Mvc;

namespace Structing.HotReload.School.Controllers
{
    [ApiController]
    [Route("school")]
    public class SchoolController:ControllerBase
    {
        private static int A = 1;
        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok(A++);
        }
    }
}
