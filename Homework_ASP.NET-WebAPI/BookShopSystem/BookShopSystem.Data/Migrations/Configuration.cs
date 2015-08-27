namespace BookShopSystem.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using BookShopSystem.Models;
    using System.Globalization;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<BookShopSystem.Data.BookShopSystemContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.ContextKey = "BookShopSystem.Data.BookShopSystemContext";
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(BookShopSystem.Data.BookShopSystemContext context)
        {
            //  This method will be called after migrating to the latest version.

            if (!context.Authors.Any())
            {
                AddAuthors(context);
            }

            if (!context.Categories.Any())
            {
                AddCategories(context);
            }

            if (!context.Books.Any())
            {
                AddBooks(context);
            }

            //Roles
            if(context.Roles.Any())
            {
                return;
            }

            if(context.Users.Any())
            {
                AddRoles(context);
            }

        }

        private static void AddRoles(BookShopSystemContext context)
        {
            var user = context.Users.First();
            var adminRole = new IdentityRole()
            {
                Name = "Admin"
            };

            var moderatorRole = new IdentityRole()
            {
                Name = "Moderator"
            };

            context.Roles.Add(adminRole);
            context.Roles.Add(moderatorRole);
            context.SaveChanges();

            adminRole.Users.Add(new IdentityUserRole()
            {
                UserId = user.Id
            });

            context.SaveChanges();
        }

        private static void AddBooks(BookShopSystemContext context)
        {
            var random = new Random();
            using (var reader = new StreamReader("../../../books.txt"))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    var data = line.Split(new[] { ' ' }, 6);
                    var authorIndex = random.Next(0, context.Authors.Count());
                    var author = context.Authors.ToList()[authorIndex];
                    var categoryIndex = random.Next(0, context.Categories.Count());
                    var category = context.Categories.ToList()[categoryIndex];
                    var edition = (EditionType)int.Parse(data[0]);
                    var releaseDate = DateTime.ParseExact(data[1], "d/M/yyyy", CultureInfo.InvariantCulture);
                    var copies = int.Parse(data[2]);
                    var price = decimal.Parse(data[3]);
                    var ageRestriction = (AgeRestriction)int.Parse(data[4]);
                    var title = data[5];

                    context.Books.Add(new Book()
                    {
                        Edition = edition,
                        ReleaseDate = releaseDate,
                        Copies = copies,
                        Price = price,
                        AgeRestriction = ageRestriction,
                        Title = title,
                        Author = author,
                        Categories = new[] { category }
                    });

                    line = reader.ReadLine();
                }
                context.SaveChanges();
            }
        }

        private static void AddCategories(BookShopSystemContext context)
        {
            using (var reader = new StreamReader("../../../categories.txt"))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    var categories = line.Split(new[] { ' ' }, 6);
                    foreach (var categorie in categories)
                    {
                        Category category = new Category()
                        {
                            Name = categorie
                        };
                        context.Categories.Add(category);
                    }
                    line = reader.ReadLine();
                }
                context.SaveChanges();
            }
        }

        private static void AddAuthors(BookShopSystemContext context)
        {
            using (var reader = new StreamReader("../../../authors.txt"))
            {
                var line = reader.ReadLine();
                line = reader.ReadLine();
                while (line != null)
                {
                    var fullName = line.Split(new[] { '\r' }, 6);
                    foreach (var s in fullName)
                    {
                        var splitFirstAndLastName = s.Split(' ');
                        var firstName = splitFirstAndLastName[0];
                        var lastName = splitFirstAndLastName[1];
                        Author author = new Author()
                        {
                            FirstName = firstName,
                            LastName = lastName
                        };
                        context.Authors.Add(author);
                    }

                    line = reader.ReadLine();
                }
                context.SaveChanges();
            }
        }
    }
}
