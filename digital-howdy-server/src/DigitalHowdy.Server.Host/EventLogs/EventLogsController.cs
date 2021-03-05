using System.Threading.Tasks;
using DigitalHowdy.Server.EventLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalHowdy.Server.Host.EventLogs
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventLogsController : ControllerBase
    {
        private readonly IEventLogDAO _eventLogDAO;

        public EventLogsController(IEventLogDAO eventLogDAO)
        {
            _eventLogDAO = eventLogDAO;
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEvents([FromQuery] string query = "")
        {
            var results = await _eventLogDAO.GetAllEvents(query);

            return Ok(results);
        }
    }
}