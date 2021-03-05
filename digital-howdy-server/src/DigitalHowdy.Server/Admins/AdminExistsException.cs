using System;

namespace DigitalHowdy.Server.Admins
{
    public class AdminExistsException : Exception
    {
        public AdminExistsException(string message) : base(message)
        {
            
        }
    }
}