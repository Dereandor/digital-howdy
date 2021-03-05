using System.Data;
using Microsoft.Extensions.Configuration;
using LightInject;
using DigitalHowdy.Server.Visits;
using DigitalHowdy.Server.Visitors;
using DigitalHowdy.Server.Test.Utils;
using DigitalHowdy.Server.Employees;
using DigitalHowdy.Server.Database;
using DigitalHowdy.Server.Admins;
using DigitalHowdy.Server.EventLogs;

namespace DigitalHowdy.Server.Test
{
    public class TestCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry registry)
        {
            var configuration = GetConfiguration();

            registry.RegisterSingleton<IConfiguration>(_ => configuration);
            registry.RegisterFrom<DatabaseCompositionRoot>();
            registry.RegisterSingleton<VisitorDAO, VisitorDAO>();
            registry.RegisterSingleton<VisitDAO, VisitDAO>();
            registry.RegisterSingleton<EmployeeDAO, EmployeeDAO>();
            registry.RegisterSingleton<AdminDAO, AdminDAO>();
            registry.RegisterSingleton<EventLogDAO, EventLogDAO>();
            registry.RegisterSingleton<TestDataCreator, TestDataCreator>();
        }

        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.test.example.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}