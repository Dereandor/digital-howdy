using System.Data;
using DbReader;

namespace DigitalHowdy.Server.Database.Migrations
{
    [AppliesToVersion(2)]
    public class CreateEventLogMigration : IMigration
    {
        private readonly ISqlQueries _sqlQueries;

        public CreateEventLogMigration(ISqlQueries sqlQueries)
            => _sqlQueries = sqlQueries;

        public void Migrate(IDbConnection dbConnection)
        {
            dbConnection.Execute(_sqlQueries.CreateEventLogTable);
        }
    }
}