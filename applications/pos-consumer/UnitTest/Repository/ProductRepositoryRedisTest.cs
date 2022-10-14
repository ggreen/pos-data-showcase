using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using pos_consumer.Domain;
using pos_consumer.Repository.Redis;
using StackExchange.Redis;

namespace UnitTest
{

    [TestClass]
    public class ProductRepositoryRedisTest
    {
        private Mock<IDatabase> db;
        private Mock<IConnectionMultiplexer> cache;

        [TestInitialize]
        public void InitializeProductRepositoryRedisTest()
        {
            db = new Mock<IDatabase>();
            cache = new Mock<IConnectionMultiplexer>();
        }
        
        [TestMethod]
        public void Given__Product_When_Save_Redis_ToRedis()
        {
            var subject = new ProductRepositoryRedis(cache.Object);
            cache.Setup(cache => cache.GetDatabase(It.IsAny<int>(),It.IsAny<object>())).Returns(db.Object);

            Product product = new Product("id","name");
            
            subject.Save(product);
            
        }
    }
}