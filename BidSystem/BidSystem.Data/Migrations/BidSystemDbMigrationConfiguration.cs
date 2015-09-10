namespace BidSystem.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using BidSystem.Data;

    public sealed class BidSystemDbMigrationConfiguration :
        DbMigrationsConfiguration<BidSystemDbContext>
    {
        public BidSystemDbMigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BidSystemDbContext context)
        {
            //  This method will be called after migrating to the latest version
        }
    }
}
