using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DbReader;
using DigitalHowdy.Server.Database;

namespace DigitalHowdy.Server.EventLogs
{
    public class EventLogDAO : IEventLogDAO
    {
        private readonly ISqlQueries _sqlQueries;
        private readonly IDbConnection _dbConnection;

        public EventLogDAO(ISqlQueries sqlQueries, IDbConnection dbConnection)
        {
            _sqlQueries = sqlQueries;
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<EventLogDTO>> GetAllEvents(string query = "")
        {
            IEnumerable<EventLogDTO> results;
            if (query != "")
            {
                results = await _dbConnection.ReadAsync<EventLogDTO>(_sqlQueries.GetAllEventLogsByQuery, new { query = $"%{query}%"});
            }
            else
            {
                results = await _dbConnection.ReadAsync<EventLogDTO>(_sqlQueries.GetAllEventLogs);
            }
            

            return results;
        }

        public async Task<EventLogDTO> InsertEvent(EventLogDTO eventLog)
        {
            await _dbConnection.ExecuteAsync(_sqlQueries.InsertEventLog, eventLog);
            var eventLogId = await _dbConnection.ExecuteScalarAsync<long>(_sqlQueries.GetLastInsertId);

            eventLog.Id = eventLogId;

            return eventLog;
        }
    }
}