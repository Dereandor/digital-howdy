using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHowdy.Server.EventLogs
{
    public interface IEventLogDAO
    {
        Task<IEnumerable<EventLogDTO>> GetAllEvents(string query = "");
        Task<EventLogDTO> InsertEvent(EventLogDTO eventLog);
    }
}