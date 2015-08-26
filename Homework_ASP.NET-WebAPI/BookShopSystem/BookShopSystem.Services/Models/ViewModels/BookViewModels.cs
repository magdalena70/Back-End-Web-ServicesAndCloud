using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BookShopSystem.Services.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public EditionType Edition { get; set; }

        public decimal Price { get; set; }

        public AgeRestriction Restriction { get; set; }

        public decimal Copies { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public AuthorViewModel Author { get; set; }

        public static Expression<Func<Book, BookViewModel>> Create
        {
            get
            {
                return b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Edition = b.Edition,
                    Price = b.Price,
                    Restriction = b.AgeRestriction,
                    Copies = b.Copies,
                    ReleaseDate = b.ReleaseDate,
                    Author = new AuthorViewModel()
                    {
                        AuthorId = b.Author.Id,
                        AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                    },
                    Categories = b.Categories.Select(c => new CategoryViewModel()
                    {
                        Name = c.Name
                    })
                };
            }
        }
    }
}