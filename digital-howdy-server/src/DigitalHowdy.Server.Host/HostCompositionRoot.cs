using System.Data;
using DigitalHowdy.Server.Admins;
using DigitalHowdy.Server.Authorization;
using DigitalHowdy.Server.Database;
using DigitalHowdy.Server.Employees;
using DigitalHowdy.Server.EventLogs;
using DigitalHowdy.Server.Messaging;
using DigitalHowdy.Server.Visitors;
using DigitalHowdy.Server.Visits;
using LightInject;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DigitalHowdy.Server.Host
{
    public class HostCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            var configuration = GetConfiguration();

            serviceRegistry.RegisterSingleton<IConfigurationRoot>(_ => configuration);

            serviceRegistry.RegisterFrom<DatabaseCompositionRoot>();

            if (configuration.GetValue<bool>("Twilio:Enabled") == true)
            {
                serviceRegistry.RegisterSingleton<ISmsService, TwilioService>();
            }

            serviceRegistry.RegisterSingleton<IUserContext, UserContext>()
                .RegisterSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .RegisterSingleton<IAdminDAO, AdminDAO>()
                .RegisterSingleton<IVisitorDAO, VisitorDAO>()
                .RegisterSingleton<IVisitDAO, VisitDAO>()
                .RegisterSingleton<IEmployeeDAO, EmployeeDAO>()
                .RegisterSingleton<IEventLogDAO, EventLogDAO>()
                .RegisterSingleton<IEventLogger, EventLogger>();
        }

        private IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}