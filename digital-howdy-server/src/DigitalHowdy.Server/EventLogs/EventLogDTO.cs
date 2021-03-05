using System;

namespace DigitalHowdy.Server.EventLogs
{
    public class EventLogDTO
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}