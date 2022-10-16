using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using StackExchange.Redis;

namespace pos_service.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        
        private readonly ILogger<ProductController> logger;

        private readonly IConnectionMultiplexer cache;

        public ProductController(ILogger<ProductController> logger,IConnectionMultiplexer cache)
        {
            this.logger = logger;

            this.cache = cache;
        }

        [HttpGet]
        [Route("{id}")]
        public String Get(string id)
        {
            logger.LogInformation($"get id: {id}");

            IDatabase db = cache.GetDatabase();
        
            return db.StringGet(id);

        }


             [HttpGet]
        [Route("json/{id}")]
        public String GetString(string id)
        {
            logger.LogInformation($"get id: {id}");

            IDatabase db = cache.GetDatabase();
        
           return db.StringGet(id);

        }

        [HttpPost]
        [Route("{id}")]
        public void SaveProduct(String id,[FromBody] String product)
        {
            IDatabase db = cache.GetDatabase();
            
            String json = JsonSerializer.Serialize(product);

            logger.LogInformation($"product: {product}");

            db.StringSet(id,json);

        }

        [HttpPost]
        [Route("json/{id}")]
        public void SaveJson(String id, String product)
        {
            IDatabase db = cache.GetDatabase();
            
            logger.LogInformation($"product: {product}");

            db.StringSet(id,product);

        }
    }
}
