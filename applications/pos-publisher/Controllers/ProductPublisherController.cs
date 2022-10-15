using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pos_publisher.Domain;
using Steeltoe.Messaging;
using Steeltoe.Messaging.Support;
using Steeltoe.Stream.Messaging;

namespace pos_publisher.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductPublisherController : ControllerBase
    {

        private readonly ILogger<ProductPublisherController> logger;
        private readonly ISource source;

        public ProductPublisherController(ILogger<ProductPublisherController> logger,ISource source)
        {
            this.logger = logger;
            this.source = source;
        }

        [HttpPost]
        public void PublishProduct([FromBody] Product value)
        {
            source.Output.Send(MessageBuilder.WithPayload(value).Build());
        }
    
    }
}
