using BookShopSystem.Data;
using BookShopSystem.Models;
using BookShopSystem.Services.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BookShopSystem.Services.Controllers
{
    public class AuthorsController : BaseApiController
    {
        // GET Api/Authors
        public IHttpActionResult GetAuthors()
        {
            var authors = this.Data.Authors
                .Select(a => new
            {
                id = a.Id,
                FullName = a.FirstName + " " + a.LastName,
                BookTitles = a.Books.Select(b => b.Title)
            });

            if (!authors.Any())
            {
                return this.NotFound();
            }

            return this.Ok(authors);
        }

        // GET Api/authors/5
        public IHttpActionResult GetAuthorById(int id)
        {
            var author = this.Data.Authors
                .Where(a => a.Id == id)
                .Select(a => new
            {
                FullName = a.FirstName + " " + a.LastName,
                BookTitles = a.Books.Select(b => b.Title)
            });

            if (!author.Any())
            {
                return this.NotFound();
            }

            return this.Ok(author);
        }

        // GET	/api/authors/{id}/books
        [Route("api/authors/{id}/books")]
        public IHttpActionResult GetBooksByAuthorId(int id)
        {
            var books = this.Data.Books
                .Where(b => b.Author.Id == id)
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(b => new
                {
                    Title = b.Title,
                    Description = b.Description,
                    Edition = b.Edition,
                    Price = b.Price,
                    Restriction = b.AgeRestriction,
                    Copies = b.Copies,
                    ReleaseDate = b.ReleaseDate,
                    Author = new
                    {
                        AuthorId = b.Author.Id,
                        AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                    },
                    Categories = b.Categories.Select(c => new
                    {
                        Name = c.Name
                    })
                });

            if (!books.Any())
            {
                return this.NotFound();
            }

            return this.Ok(books);
        }

        // POST api/Authors
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddAuthor(AuthorBindingModel model)
        {

            if (model == null)
            {
                this.ModelState.AddModelError("model", "The model cannot be null - (request is empty).");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var author = new Author()
            {
                FirstName = model.FirstName ?? null,
                LastName = model.LastName
            };

            this.Data.Authors.Add(author);
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}