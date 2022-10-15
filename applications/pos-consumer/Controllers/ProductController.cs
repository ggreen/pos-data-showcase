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
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> logger;
        private readonly  ISource source;

        public ProductController(ILogger<ProductController> logger, ISource source)
        {
            this.logger = logger;
            this.source = source;
        }

        [HttpGet]
        public IEnumerable<Product> Get()
        {
            logger.LogInformation("Getting product");
           Product[] products = { new Product("id","name")};
           return products;
        }
    }
}
