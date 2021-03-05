using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalHowdy.Server.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalHowdy.Server.Host.Employees
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeDAO _employeeDAO;

        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeDAO emplyeeDAO)
        {
            _logger = logger;
            _employeeDAO = emplyeeDAO;
        }

        /// <summary>
        /// Gets all Employee names and Id's
        /// </summary>
        [HttpGet("names")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEmployessNamesAndId()
        {
            var results = await _employeeDAO.GetAllEmployeeNamesAndId();

            return Ok(results);

        }

        /// <summary>
        /// Get Employee by input ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployeeById([FromRoute] long id)
        {
            var results = await _employeeDAO.GetEmployeeById(id);

            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get employee by input name, if no name, return all employees
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployee([FromQuery] string name)
        {
            IEnumerable<EmployeeDTO> results = null;

            if (name == null)
            {
                results = await _employeeDAO.GetAllEmployee();
            }
            else
            {
                results = await _employeeDAO.GetEmployeeByName(name);
            }


            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Select Employee by ID number and update phone number.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateEmployeePhone([FromRoute] long id, [FromBody] string phone)
        {
            var results = await _employeeDAO.UpdateEmployeePhone(id, phone);

            if (results)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Insert new omployee into system by sending an an employee object
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertEmployee([FromBody] EmployeeDTO newEmployee)
        {
            var created = await _employeeDAO.InsertEmployee(newEmployee);

            return Created(nameof(GetEmployeeById), created);
        }

        /// <summary>
        /// Insert an array of new employees
        /// </summary>
        /// <param name="newEmployees"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InsertMultipleEmployees([FromBody] EmployeeDTO[] newEmployees)
        {
            foreach (var employee in newEmployees)
            {
                var created = await _employeeDAO.InsertEmployee(employee);
                employee.Id = created.Id;
            }

            return Created(nameof(GetAllEmployessNamesAndId), newEmployees);
        }
    }
}
