using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BookShopSystem.Services.Models.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorId { get; set; }

        public string AuthorFullName { get; set; }
    }

    public class AuthorWithBooksViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<string> BookTitles { get; set; }

        public static Expression<Func<Author, AuthorWithBooksViewModel>> Create
        {
            get
            {
                return a => new AuthorWithBooksViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    BookTitles = a.Books.Select(b => b.Title)
                };
            }
        }
    }
}