using System;

namespace DigitalHowdy.Server.Employees
{
    public class EmployeeNotFoundException : Exception
    {
        public EmployeeNotFoundException(string message) : base(message)
        {
            
        }
    }
}