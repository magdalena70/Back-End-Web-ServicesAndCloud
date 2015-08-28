namespace OnlineShop.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using OnlineShop.Data.Migrations;
    using OnlineShop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class OnlineShopContext : IdentityDbContext<ApplicationUser>
    {
        public OnlineShopContext()
            : base("OnlineShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OnlineShopContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions
                .Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public static OnlineShopContext Create()
        {
            return new OnlineShopContext();
        }

        public virtual DbSet<Ad> Ads { get; set; }
        public virtual DbSet<AdType> AdTypes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}