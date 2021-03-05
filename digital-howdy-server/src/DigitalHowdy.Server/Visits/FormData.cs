using DigitalHowdy.Server.Employees;

namespace DigitalHowdy.Server.Visits
{
    public class FormData
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}