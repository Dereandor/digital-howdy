using System;

namespace DigitalHowdy.Server.Authentication
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message)
        {
            
        }
    }
}