using DigitalHowdy.Server.Employees;
using DigitalHowdy.Server.Visitors;
using System;

namespace DigitalHowdy.Server.Visits
{
    public class VisitDTO
    {
        public long Id { get; set; }
        public VisitorDTO Visitor { get; set; }
        public EmployeeDTO Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long Reference { get; set; }
    }
}