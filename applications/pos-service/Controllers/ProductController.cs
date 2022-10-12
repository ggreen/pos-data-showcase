using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pos_service.Domain;
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
        public Product Get(string id)
        {
            logger.LogInformation($"get id: {id}");

            IDatabase db = cache.GetDatabase();
        
            String productJson = db.StringGet(id);

            if(productJson == null)
                return null;

            var product = JsonSerializer.Deserialize<Product>(productJson);
            return product;
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
        public void SaveProduct(Product product)
        {
            IDatabase db = cache.GetDatabase();
            
            String json = JsonSerializer.Serialize(product);

            logger.LogInformation($"product: {product}");

            // I tried this first: cache.Set(product.id,stream.ToArray());
            db.StringSet(product.id,json);

        }

        [HttpPost]
        [Route("json/{id}")]
        public void SaveJson(String id, String product)
        {
            IDatabase db = cache.GetDatabase();
            

            logger.LogInformation($"product: {product}");

            // I tried this first: cache.Set(product.id,stream.ToArray());
            db.StringSet(id,product);

        }
    }
}
