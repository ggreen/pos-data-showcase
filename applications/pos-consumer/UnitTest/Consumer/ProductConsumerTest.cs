
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using pos_consumer.Domain;
using pos_consumer.Consumer;
using pos_consumer.Repository;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace pos_service.UnitTest.Consumer
{
    [TestClass]
   public class ProductConsumerTest
   {
        private Mock<IProductRepository> repository;
        private ProductConsumer subject;

                    // Given
        private Product product = new Product("id","name",3.434,"details","ingredients", "directions",
                                    "warnings",
                                    "quantityAmount",
                                    new Nutrition(3,3, 3, 3,3,3));
        private Mock<IModel> channel;

        [TestInitialize]
        public void InitializeProductConsumerTest()
        {
            channel = new Mock<IModel>();

            repository = new Mock<IProductRepository>();

            subject = new ProductConsumer(repository.Object);
        }

        [TestMethod]
        public void Given_Product_When_save_then_cache()
        {

            // When
            subject.SaveProduct(product);

            // Then
            repository.Verify( r => r.Save(It.IsAny<Product>()));


        }

        [TestMethod]
        public void Given_mesasge_When_ReceiveMessage_then_save()
        {
                        // When
            subject.ReceiveMessage(channel.Object, "{}", new BasicDeliverEventArgs());

            // Then
            repository.Verify( r => r.Save(It.IsAny<Product>()));

        }
    }
    
}