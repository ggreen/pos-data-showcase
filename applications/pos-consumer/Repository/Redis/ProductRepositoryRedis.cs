using System.Text.Json;
using pos_consumer.Domain;
using StackExchange.Redis;

namespace pos_consumer.Repository.Redis
{
    public class ProductRepositoryRedis : IProductRepository
    {
        private IConnectionMultiplexer cache;

        public ProductRepositoryRedis(IConnectionMultiplexer cache)
        {
            this.cache = cache;
        }

        public void Save(Product product)
        {
           var db = cache.GetDatabase();
        
             db.StringSet(product.id, JsonSerializer.Serialize(product));
        }
    }

}