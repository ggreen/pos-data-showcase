
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using pos_consumer.Domain;
using pos_consumer.Consumer;
using pos_consumer.Repository;
using System;

namespace pos_service.UnitTest.Consumer
{
    [TestClass]
   public class ProductConsumerTest
   {
        private Mock<IProductRepository> repository;

        [TestInitialize]
        public void InitializeProductConsumerTest()
        {
            repository = new Mock<IProductRepository>();
        }

        [TestMethod]
        public void Given_Product_When_save_then_cache()
        {
            // Given
            var product = new Product("id","name");

            var subject = new ProductConsumer(repository.Object);
        
            // When
            subject.SaveProduct(product);

            // Then
            repository.Verify( r => r.Save(It.IsAny<Product>()));

        }
    }
    
}