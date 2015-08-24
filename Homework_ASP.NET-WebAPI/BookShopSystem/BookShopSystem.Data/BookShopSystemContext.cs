namespace BookShopSystem.Data
{
    using BookShopSystem.Data.Migrations;
    using BookShopSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class BookShopSystemContext : IdentityDbContext<ApplicationUser>
    {
        public BookShopSystemContext()
            : base("BookShopSystemContext")
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<BookShopSystemContext,
                    Configuration>());
        }

        public static BookShopSystemContext Create()
        {
            return new BookShopSystemContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(r => r.RelatedBooks)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("BookId");
                    m.MapRightKey("RelatedBookId");
                    m.ToTable("BooksRelatedBooks");

                });

            modelBuilder.Conventions
                .Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public virtual IDbSet<Book> Books { get; set; }
        public virtual IDbSet<Author> Authors { get; set; }
        public virtual IDbSet<Category> Categories { get; set; }
        public virtual IDbSet<Purchase> Purchases { get; set; }

    }
}