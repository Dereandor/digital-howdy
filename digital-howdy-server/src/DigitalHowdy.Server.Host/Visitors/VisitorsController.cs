using System.Threading.Tasks;
using DigitalHowdy.Server.Visitors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalHowdy.Server.Host.Visitors
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorsController : ControllerBase
    {
        private readonly ILogger<VisitorsController> _logger;
        private readonly IVisitorDAO _visitorDAO;

        public VisitorsController(ILogger<VisitorsController> logger, IVisitorDAO visitorDAO)
        {
            _logger = logger;
            _visitorDAO = visitorDAO;
        }

        /// <summary>
        /// Sends a quesry to return list of visitors with queried phone number, if query is blank return all visitors
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVisitors([FromQuery] string query = "", [FromQuery] string phone = "")
        {
            if (phone == "")
            {
                var results = await _visitorDAO.GetAllVisitors(query);
                return Ok(results);
            }
            else
            {
                var result = await _visitorDAO.GetVisitorByPhone(phone);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }

            }
        }


    }
}