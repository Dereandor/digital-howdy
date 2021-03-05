using System;
using System.Data;
using System.Linq;
using DbReader;
using DigitalHowdy.Server.Database.Migrations;
using DigitalHowdy.Server.Database.Version;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace DigitalHowdy.Server.Database
{
    public class DatabaseMigrator : IDatabaseMigrator
    {
        private readonly IConfiguration _configuration;
        private readonly ISqlQueries _sqlQueries;
        private readonly ILogger<DatabaseMigrator> _logger;

        public DatabaseMigrator(IConfiguration configuration, ISqlQueries sqlQueries, ILogger<DatabaseMigrator> logger)
        {
            _configuration = configuration;
            _sqlQueries = sqlQueries;
            _logger = logger;
        }

        public void Migrate()
        {
            using (var connection = new MySqlConnection(_configuration["ConnectionString"]))
            {
                connection.Open();
                var appliedVersions = GetDatabaseVersion(connection);
                var databaseVersion = appliedVersions.OrderBy(vi => vi.Version).LastOrDefault()?.Version ?? 0;
                _logger.LogInformation($"Database is at version {databaseVersion}");

                var migrationsByVersion = GetMigrationsByVersion(databaseVersion);

                foreach (var migrations in migrationsByVersion)
                {
                    var appliedMigrationsDescription = string.Empty;

                    _logger.LogInformation($"Found {migrations.Count()} migration(s) to be applied for version {migrations.Key}");

                    foreach (var migration in migrations.OrderBy(m => m.Order))
                    {
                        _logger.LogInformation($"Applying information {migration.MigrationType.Name}");
                        var migrationInstance = (IMigration)Activator.CreateInstance(migration.MigrationType, _sqlQueries);
                        migrationInstance.Migrate(connection);
                        if (string.IsNullOrWhiteSpace(appliedMigrationsDescription))
                        {
                            appliedMigrationsDescription = migration.MigrationType.Name;
                        }
                        else
                        {
                            appliedMigrationsDescription = $"{appliedMigrationsDescription}, {migration.MigrationType.Name}";
                        }

                        connection.Execute(_sqlQueries.InsertVersionInfo, new VersionInfo() { Version = migrations.Key });
                        _logger.LogInformation($"Database is now at version {migrations.Key}");
                    }
                }
                
                connection.Close();
            }
        }

        private VersionInfo[] GetDatabaseVersion(IDbConnection connection)
        {
            var stuff = connection.Read<string>(_sqlQueries.IsEmptyDatabase);
            var isEmpty = connection.Read<string>(_sqlQueries.IsEmptyDatabase).Count() == 0;
            if (isEmpty)
            {
                return Array.Empty<VersionInfo>();
            }
            else
            {
                return connection.Read<VersionInfo>(_sqlQueries.GetVersionInfo).ToArray();
            }
        }

        private IGrouping<int, MigrationInfo>[] GetMigrationsByVersion(long databaseVersion)
        {
            return typeof(DatabaseMigrator).Assembly.GetTypes()
                .Where(t => typeof(IMigration).IsAssignableFrom(t) && t.IsClass)
                .Select(t => new MigrationInfo(t,
                    ((AppliesToVersionAttribute)t.GetCustomAttributes(typeof(AppliesToVersionAttribute), true).Single()).Version,
                    ((AppliesToVersionAttribute)t.GetCustomAttributes(typeof(AppliesToVersionAttribute), true).Single()).Order)
                )
                .GroupBy(m => m.AppliesToVersion)
                .Where(g => g.Key > databaseVersion).OrderBy(g => g.Key).ToArray();
        }
    }
}