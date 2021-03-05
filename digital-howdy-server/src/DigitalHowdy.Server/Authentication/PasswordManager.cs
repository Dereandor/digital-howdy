using Microsoft.AspNetCore.Identity;

namespace DigitalHowdy.Server.Authentication
{
    public class PasswordManager : IPasswordManager
    {
        private readonly IPasswordHasher<PasswordManager> _passwordHasher;

        public PasswordManager()
        {
            _passwordHasher = new PasswordHasher<PasswordManager>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(this, password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(this, hashedPassword, password) == PasswordVerificationResult.Success;
        }
    }
}