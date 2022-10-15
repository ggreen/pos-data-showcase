using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using pos_publisher.Controllers;
using Steeltoe.Stream.Extensions;

namespace pos_publisher
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //  var host = StreamHost
            //   .CreateDefaultBuilder<Program>(args)
            //   .ConfigureWebHostDefaults(webhostBuilder => webhostBuilder.UseStartup<Startup>())
            // //   .ConfigureServices(svc => svc.AddHostedService<Worker>())
            //   .Build();

            CreateHostBuilder(args)
            .AddStreamServices<ProductPublisherController>()
            .Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
