using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace pos_consumer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class HealthCheckController : ControllerBase
    {

        private readonly ILogger<HealthCheckController> logger;

        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
          return Ok();
        }
    }
}
