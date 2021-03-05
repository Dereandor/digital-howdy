using System;

namespace DigitalHowdy.Server.Database.Migrations
{
    public class MigrationInfo
    {
        public Type MigrationType { get; }
        public int AppliesToVersion { get; }
        public int Order { get; }

        public MigrationInfo(Type migrationType, int appliesToVersion, int order)
        {
            MigrationType = migrationType;
            AppliesToVersion = appliesToVersion;
            Order = order;
        }
    }
}