using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Steeltoe.Stream.Extensions;
using pos_consumer.Consumer;

namespace pos_consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).
            AddStreamServices<ProductConsumer>().
            Build().Run();

            // var builder = WebApplication.CreateBuilder(args);
            // builder.Host.AddStreamServices<ProduceConsumer>();
            // builder.Build().Run();

            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
