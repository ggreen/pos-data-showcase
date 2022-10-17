using Imani.Solutions.Core.API.Util;
using Imani.Solutions.RabbitMQ.API;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using pos_consumer.Consumer;
using pos_consumer.Repository;
using pos_consumer.Repository.Redis;
using StackExchange.Redis;

namespace pos_consumer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var config  = new ConfigSettings();
            var connectionStrings = config.GetProperty("Redis_Connection_String");

            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
                connectionStrings);

            services.AddSingleton(redis);

            IProductRepository productRepository = new ProductRepositoryRedis(redis);
            
            services.AddSingleton(productRepository);
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "pos_consumer", Version = "v1" });
            });


             var rabbit = Rabbit.Connect();
             var consumer = rabbit.ConsumerBuilder()
                        .SetExchange("pos.products")
                        .UseQueueType(RabbitQueueType.quorum)
                        .AddQueue("pos.products.consumer","#").Build();

             var productConsumer = new ProductConsumer(productRepository);
             consumer.RegisterReceiver(productConsumer.ReceiveMessage);

              services.AddSingleton(productConsumer);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "pos_consumer"));
            }

            
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
