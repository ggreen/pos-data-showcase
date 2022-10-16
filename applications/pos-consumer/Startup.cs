using System.Collections.Generic;
using System.Net;
using Imani.Solutions.Core.API.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using pos_consumer.Repository;
using pos_consumer.Repository.Redis;
using StackExchange.Redis;
using Steeltoe.Connector.RabbitMQ;
using Steeltoe.Connector.Redis;

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
            
            services.AddRabbitMQConnection(Configuration);
            services.AddDistributedRedisCache(Configuration);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "pos_consumer", Version = "v1" });
            });


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
