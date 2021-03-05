using System.Collections.Generic;
using System.Security.Claims;

namespace DigitalHowdy.Server.Authorization
{
    public interface ITokenManager
    {
        string GenerateToken(IEnumerable<Claim> claims);
    }
}