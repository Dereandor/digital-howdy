using System.Data;

namespace DigitalHowdy.Server.Database.Migrations
{
    public interface IMigration
    {
        void Migrate(IDbConnection dbConnection);
    }
}