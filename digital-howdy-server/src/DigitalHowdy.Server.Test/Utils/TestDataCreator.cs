using System;
using System.Threading.Tasks;
using System.Data;
using DbReader;
using DigitalHowdy.Server.Database;

namespace DigitalHowdy.Server.Test.Utils
{
    public class TestDataCreator
    {
        private readonly IDbConnection _dbConnection;
        private readonly ISqlQueries _sqlQueries;
        public TestDataCreator(IDbConnection dbConnection, ISqlQueries sqlQueries)
        {
            _dbConnection = dbConnection;
            _sqlQueries = sqlQueries;
        }

        public async Task CreateTestData()
        {
            await _dbConnection.ExecuteAsync(_sqlQueries.CreateTables);
            await _dbConnection.ExecuteAsync(_sqlQueries.CreateEventLogTable);
            await _dbConnection.ExecuteAsync(_sqlQueries.CreateTestData);
        }
    }
}