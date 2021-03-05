using System;
using System.Threading.Tasks;
using DigitalHowdy.Server.Employees;
using DigitalHowdy.Server.EventLogs;
using DigitalHowdy.Server.Messaging;
using DigitalHowdy.Server.Visitors;
using DigitalHowdy.Server.Visits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DigitalHowdy.Server.Host.Visits
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IEventLogger _logger;
        private readonly IVisitDAO _visitDAO;
        private readonly IVisitorDAO _visitorDAO;
        private readonly IEmployeeDAO _employeeDAO;
        private readonly ISmsService _smsService;

        private readonly string _checkInMessage;

        public VisitsController(IConfiguration configuration, IEventLogger logger, IVisitDAO visitDAO, IVisitorDAO visitorDAO, IEmployeeDAO employeeDAO, ISmsService smsService = null)
        {
            _logger = logger;
            _visitDAO = visitDAO;
            _visitorDAO = visitorDAO;
            _smsService = smsService;
            _employeeDAO = employeeDAO;
            _checkInMessage = configuration?["General:CheckInMessage"];
        }

        /// <summary>
        /// Return a list of all visits
        /// </summary>
        //[Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllVisits([FromQuery] string query = "", [FromQuery] bool exact = false, [FromQuery] bool currentlyIn = false)
        {
            var results = await _visitDAO.GetAllVisits(query, exact, currentlyIn);

            return Ok(results);
        }

        /// <summary>
        /// Return a visit by querying visit id
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVisitById([FromRoute] long id)
        {
            var result = await _visitDAO.GetVisitById(id);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Register a new visit
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterVisit([FromBody] FormData formData)
        {
            try
            {
                var insertVisitor = new VisitorInsertDTO
                {
                    Name = formData.Name,
                    Phone = formData.Phone,
                    OrganizationName = formData.Organization
                };

                var visitor = await _visitorDAO.InsertVisitor(insertVisitor);

                var rng = new Random();

                var insertVisit = new VisitInsertDTO
                {
                    VisitorId = visitor.Id,
                    EmployeeId = formData.Employee.Id,
                    StartDate = DateTime.Now,
                    Reference = rng.Next(10000)
                };

                var insertedVisit = await _visitDAO.InsertVisit(insertVisit);

                var result = new VisitReferenceDTO
                {
                    Id = insertedVisit.Id,
                    Reference = insertedVisit.Reference
                };

                if (_smsService != null)
                {
                    var employee = await _employeeDAO.GetEmployeeById(formData.Employee.Id);

                    var sms = new SmsMessage(employee.Phone, string.Format(_checkInMessage, visitor.Name));

                    _smsService.SendSMS(sms);
                }

                if (_logger != null) await _logger.LogCheckin(visitor.Name);

                return Ok(result);
            }
            catch (EmployeeNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Add end date to a visit when visit completes
        /// </summary>
        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEndDateVisit([FromBody] VisitUpdateDTO newUpdate)
        {
            var visit = await _visitDAO.GetVisitById(newUpdate.Id);
            var result = await _visitDAO.UpdateEndDateVisit(newUpdate);

            if (result)
            {
                if (_logger != null) await _logger.LogCheckout(visit.Visitor.Name);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Delete a visit from database
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVisit([FromRoute] long id)
        {
            var result = await _visitDAO.DeleteVisit(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}