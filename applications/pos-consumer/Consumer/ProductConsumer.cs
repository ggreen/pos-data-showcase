using Imani.Solutions.RabbitMQ.API;
using pos_consumer.Domain;
using pos_consumer.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;

namespace pos_consumer.Consumer
{
    public class ProductConsumer{
        private readonly IProductRepository repository;

        private readonly RabbitConsumer consumer;

        public ProductConsumer(IProductRepository repository)
        {
            this.repository = repository;

            Console.WriteLine($"Debug: repository {repository}");

        }

        public void SaveProduct(Product product)
        {
            Console.WriteLine("Received: " + product);
            repository.Save(product);
        }


        public void ReceiveMessage(IModel channel, object message, BasicDeliverEventArgs eventArg)
        {
            try{

                var body = eventArg.Body;
                String contentType = eventArg.BasicProperties.ContentType;

                var json = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine($"message {message}");
                System.Diagnostics.Debug.WriteLine( $"message {message}" );
                
                 var product = JsonSerializer.Deserialize<Product>(json);

                this.SaveProduct(product);
                channel.BasicAck(eventArg.DeliveryTag,false);
            }
            catch (Exception e){

                Console.WriteLine(e.StackTrace);
                channel.BasicNack(eventArg.DeliveryTag,false,true);
            }
        }
    }
}
