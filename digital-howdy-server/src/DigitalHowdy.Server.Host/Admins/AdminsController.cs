using System;
using System.Threading.Tasks;
using DigitalHowdy.Server.Admins;
using DigitalHowdy.Server.Authentication;
using DigitalHowdy.Server.EventLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DigitalHowdy.Server.Host.Admins
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminsController : ControllerBase
    {
        private readonly IEventLogger _logger;
        private readonly IAdminDAO _adminDAO;
        public AdminsController(IEventLogger logger, IAdminDAO adminDAO)
        {
            _logger = logger;
            _adminDAO = adminDAO;
        }

        /// <summary>
        /// Gets a list of all admins
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAdmins()
        {
            var results = await _adminDAO.GetAllAdmins();

            return Ok(results);
        }

        // TODO: Need admin privileges to register new admin?
        /// <summary>
        /// Adds a new Admin account to the system.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDTO adminRegister)
        {
            try
            {
                var insertedAdmin = await _adminDAO.RegisterAdmin(adminRegister);

                if (_logger != null) await _logger.LogAddAdmin(insertedAdmin.Username);

                return Created(nameof(insertedAdmin), insertedAdmin);
            }
            catch (AdminExistsException e)
            {
                return Conflict(new { message = e.Message });
            }
            catch (ValidationErrorException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        /// <summary>
        /// Sends in credentials to log in as administrator.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAdmin([FromBody] AdminLoginDTO adminLogin)
        {
            try
            {
                var token = await _adminDAO.LoginAdmin(adminLogin);

                if (_logger != null) await _logger.LogLogin(adminLogin.Username);

                return Ok(new { token = token });
            }
            catch (AuthenticationFailedException e)
            {
                return Unauthorized(new { message = e.Message });
            }
        }

        /// <summary>
        /// Checks validity
        /// </summary>
        [Authorize]
        [HttpPost("validate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Validate()
        {
            return Ok();
        }

        /// <summary>
        /// Deletes an admin account from the system
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAdmin([FromRoute] long id)
        {
            var result = await _adminDAO.DeleteAdmin(id);

            if (result)
            {
                if (_logger != null) await _logger.LogRemoveAdmin("");
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}