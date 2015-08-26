using BookShopSystem.Data;
using BookShopSystem.Models;
using BookShopSystem.Services.Models.BindingModels;
using BookShopSystem.Services.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BookShopSystem.Services.Controllers
{
    public class AuthorsController : BaseApiController
    {
        // GET api/Authors
        public IHttpActionResult GetAuthors()
        {
            var authors = this.Data.Authors
                .Select(AuthorWithBooksViewModel.Create);

            if (!authors.Any())
            {
                return this.NotFound();
            }

            return this.Ok(authors);
        }

        // GET api/Authors/5
        public IHttpActionResult GetAuthorById(int id)
        {
            var author = this.Data.Authors
                .Where(a => a.Id == id)
                .Select(AuthorWithBooksViewModel.Create);

            if (!author.Any())
            {
                return this.NotFound();
            }

            return this.Ok(author);
        }

        // GET	/api/Authors/{id}/Books
        [Route("api/Authors/{id}/Books")]
        public IHttpActionResult GetBooksByAuthorId(int id)
        {
            var books = this.Data.Books
                .Where(b => b.Author.Id == id)
                .OrderBy(b => b.Title)
                .Take(10)
                .Select(BookViewModel.Create);

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