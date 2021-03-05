using System;

namespace DigitalHowdy.Server.Authentication
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException(string message) : base(message)
        {

        }
    }
}