namespace DigitalHowdy.Server.Visitors
{
    public class VisitorInsertDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
    }
}