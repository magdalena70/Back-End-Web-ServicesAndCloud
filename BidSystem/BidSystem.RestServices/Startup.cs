using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BidSystem.RestServices.Startup))]

namespace BidSystem.RestServices
{
    using System.Data.Entity;

    using BidSystem.Data;
    using BidSystem.Data.Migrations;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BidSystemDbContext, BidSystemDbMigrationConfiguration>());
            ConfigureAuth(app);
        }
    }
}
