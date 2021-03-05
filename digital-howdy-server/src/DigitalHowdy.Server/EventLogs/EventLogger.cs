using System;
using System.Threading.Tasks;
using DigitalHowdy.Server.Authorization;

namespace DigitalHowdy.Server.EventLogs
{
    public class EventLogger : IEventLogger
    {
        private readonly IEventLogDAO _eventLogDAO;
        private readonly IUserContext _userContext;

        public EventLogger(IEventLogDAO eventLogDAO, IUserContext userContext)
        {
            _eventLogDAO = eventLogDAO;
            _userContext = userContext;
        }
        public async Task LogAddAdmin(string newAdminName)
        {
            var description = $"New user '{newAdminName}' was added by user '{_userContext.Username}'.";
            await InsertLog(description);
        }

        public async Task LogCheckin(string visitorName)
        {
            var description = $"Visitor '{visitorName}' checked in.";
            await InsertLog(description);
        }

        public async Task LogCheckout(string visitorName)
        {
            var description = $"Visitor '{visitorName}' was checked out by user '{_userContext.Username}.";
            await InsertLog(description);
        }

        public async Task LogLogin(string adminName)
        {
            var description = $"User '{adminName}' logged in.";
            await InsertLog(description);
        }

        public async Task LogRemoveAdmin(string removedAdminName)
        {
            var description = $"User '{removedAdminName} was deleted by user '{_userContext.Username}'.";
            await InsertLog(description);
        }

        private async Task InsertLog(string description)
        {
            var eventLog = new EventLogDTO
            {
                Date = DateTime.Now,
                Description = description
            };

            await _eventLogDAO.InsertEvent(eventLog);
        }
    }
}