using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using GenericHost = Microsoft.Extensions.Hosting.Host;

namespace DigitalHowdy.Server.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            GenericHost.CreateDefaultBuilder(args)
                .UseLightInject(services => services.RegisterFrom<HostCompositionRoot>())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
