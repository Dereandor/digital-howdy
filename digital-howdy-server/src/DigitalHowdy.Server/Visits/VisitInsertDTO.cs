using System;

namespace DigitalHowdy.Server.Visits
{
    public class VisitInsertDTO
    {
        public long Id { get; set; }

        public long VisitorId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public long Reference { get; set; }
    }
}