using System;

namespace DigitalHowdy.Server.Database.Migrations
{
    public class AppliesToVersionAttribute : Attribute
    {
        public int Version { get; }
        public int Order { get; set; }
        public AppliesToVersionAttribute(int version) => Version = version;
    }
}