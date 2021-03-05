namespace DigitalHowdy.Server.Visitors
{
    public class VisitorDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public OrganizationDTO Organization { get; set; }
    }
}