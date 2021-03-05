using System.Threading.Tasks;

namespace DigitalHowdy.Server.EventLogs
{
    public interface IEventLogger
    {
        Task LogCheckin(string visitorName);
        Task LogCheckout(string visitorName);
        Task LogLogin(string adminName);
        Task LogAddAdmin(string newAdminName);
        Task LogRemoveAdmin(string removedAdminName);
    }
}