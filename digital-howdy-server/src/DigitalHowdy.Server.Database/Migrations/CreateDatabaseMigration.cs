using System.Data;
using DbReader;

namespace DigitalHowdy.Server.Database.Migrations
{
    [AppliesToVersion(1)]
    public class CreateDatabaseMigration : IMigration
    {
        private readonly ISqlQueries _sqlQueries;

        public CreateDatabaseMigration(ISqlQueries sqlQueries)
            => _sqlQueries = sqlQueries;

        public void Migrate(IDbConnection dbConnection)
        {
            dbConnection.Execute(_sqlQueries.CreateTables);
            dbConnection.Execute(_sqlQueries.InsertAdminUser);
        }
    }
}