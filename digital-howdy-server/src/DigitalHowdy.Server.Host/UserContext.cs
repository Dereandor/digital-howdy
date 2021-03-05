using System.Security.Claims;
using DigitalHowdy.Server.Authorization;
using Microsoft.AspNetCore.Http;

namespace DigitalHowdy.Server.Host
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Username => _httpContextAccessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.GivenName).Value;
    }
}