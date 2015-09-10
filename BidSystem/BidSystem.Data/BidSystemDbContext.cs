namespace BidSystem.Data
{
    using System.Data.Entity;

    using BidSystem.Data.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class BidSystemDbContext : IdentityDbContext<User>
    {
        public BidSystemDbContext()
            : base("BidSystem")
        {
        }
        
        public static BidSystemDbContext Create()
        {
            return new BidSystemDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(user => user.Offers)
                .WithRequired(offer => offer.Seller)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<User>()
                .HasMany(user => user.Bids)
                .WithRequired(bid => bid.Bidder)
                .WillCascadeOnDelete(false);
        }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Bid> Bids { get; set; }
    }
}
