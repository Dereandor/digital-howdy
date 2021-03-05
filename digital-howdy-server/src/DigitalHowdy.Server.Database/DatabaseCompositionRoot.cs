using LightInject;
using Microsoft.Extensions.Configuration;
using DbReader;
using MySqlConnector;
using ResourceReader;
using System.Data;
using System;

namespace DigitalHowdy.Server.Database
{
    public class DatabaseCompositionRoot : ICompositionRoot
    {
        static DatabaseCompositionRoot()
        {
            DbReaderOptions.WhenReading<long?>().Use((rd, i) => rd.GetInt32(i));
            DbReaderOptions.WhenReading<long>().Use((rd, i) => rd.GetInt32(i));
            DbReaderOptions.WhenReading<string>().Use((rd, i) => (string)rd.GetValue(i));
            DbReaderOptions.WhenReading<bool>().Use((rd, i) => rd.GetInt32(i) != 0);
            DbReaderOptions.WhenReading<DateTime>().Use((rd, i) => rd.GetDateTime(i));
        }

        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry
                .RegisterSingleton(CreateDatabaseConnection)
                .RegisterSingleton<IDatabaseMigrator, DatabaseMigrator>()
                .RegisterSingleton<ISqlQueries>(_ => new ResourceBuilder().Build<ISqlQueries>());
                
        }

        private IDbConnection CreateDatabaseConnection(IServiceFactory serviceFactory)
        {
            var configuration = serviceFactory.GetInstance<IConfiguration>();
            var connection = new MySqlConnection(configuration["ConnectionString"]);

            connection.Open();

            return connection;
        }
    }
}