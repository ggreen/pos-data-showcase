using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pos_consumer.Domain;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Messaging;

namespace pos_consumer.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class HealthCheckController : ControllerBase
    {

        private readonly ILogger<HealthCheckController> logger;
        private readonly  ISource source;

        public HealthCheckController(ILogger<HealthCheckController> logger, ISource source)
        {
            this.logger = logger;
            this.source = source;
        }

        [HttpGet]
        public IActionResult Get()
        {
          return Ok();
        }
    }
}
