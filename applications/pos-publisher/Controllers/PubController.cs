
using Microsoft.AspNetCore.Mvc;
using Imani.Solutions.RabbitMQ.API;
using pos_consumer.Domain;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Logging;

namespace pos_publisher.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class PubController : ControllerBase
    {
        private readonly RabbitPublisher publisher;
        private readonly ILogger<PubController> logger;

        public PubController(RabbitPublisher publisher, ILogger<PubController> logger){
            this.publisher = publisher;
            this.logger = logger;
        }

        public void Publish(Product product)
        {
            var json = JsonSerializer.Serialize(product);
            logger.LogInformation($"json: {json}");
            this.publisher.Publish(Encoding.UTF8.GetBytes(json),product.id);
        }

    }
}