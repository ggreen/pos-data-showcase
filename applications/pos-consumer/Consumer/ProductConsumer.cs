using pos_consumer.Domain;
using pos_consumer.Repository;
using Steeltoe.Messaging.RabbitMQ.Attributes;
using Steeltoe.Stream.Attributes;
using Steeltoe.Stream.Messaging;
using System;

namespace pos_consumer.Consumer
{
      [EnableBinding(typeof(ISink))]
    public class ProductConsumer{
        private IProductRepository repository;

        public ProductConsumer(IProductRepository repository)
        {
            this.repository = repository;
        }

        [StreamListener(ISink.INPUT)]
        [DeclareQueue(Declare = "False")]
        public void SaveProduct(Product product)
        {
            Console.WriteLine("Received: " + product);
            repository.Save(product);
        }
    }
}