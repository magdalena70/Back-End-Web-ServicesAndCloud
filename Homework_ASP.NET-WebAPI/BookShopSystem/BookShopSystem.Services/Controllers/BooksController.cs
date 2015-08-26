using BookShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BookShopSystem.Models;
using BookShopSystem.Services.Models.BindingModels;

namespace BookShopSystem.Services.Controllers
{
    public class BooksController : BaseApiController
    {
        // GET api/Books
        public IHttpActionResult GetBooks()
        {
            var books = this.Data.Books
                .Select(b => new
                {
                    Id = b.Id,
                    Title = b.Title,
                    Categories = b.Categories.Select(c => c.Name),
                    Edition = b.Edition,
                    Price = b.Price,
                    Description = b.Description ?? null,
                    Restriction = b.AgeRestriction,
                    Copies = b.Copies,
                    ReleaseDate = b.ReleaseDate,
                    Author = new 
                    {
                        Id = b.Author.Id,
                        FullName = b.Author.FirstName + " " + b.Author.LastName
                    }
                });

            if(!books.Any())
            {
                return this.NotFound();
            }

            return this.Ok(books);
        }

        // GET api/Books/{id}
        public IHttpActionResult GetBookById(int id)
        {
            var book = this.Data.Books
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    Title = b.Title,
                    Categories = b.Categories.Select(c => c.Name),
                    Edition = b.Edition,
                    Price = b.Price,
                    Description = b.Description ?? null,
                    Restriction = b.AgeRestriction,
                    Copies = b.Copies,
                    ReleaseDate = b.ReleaseDate,
                    Author = new
                    {
                        Id = b.Author.Id,
                        FullName = b.Author.FirstName  + " " + b.Author.LastName
                    }
                });

            if (!book.Any())
            {
                return this.NotFound();
            }

            return this.Ok(book);
        }

        // GET /api/books?search={word}
        public IHttpActionResult GetBooksBySearchWord(string searchWord)
        {
            var books = this.Data.Books
                .Where(b => b.Title.Contains(searchWord))
                .Select(b => new
            {
                Id = b.Id,
                Title = b.Title
            });

            if (!books.Any())
            {
                return this.NotFound();
            }

            return this.Ok(books);
        }

        //PUT /api/Books/{id}
        [HttpPut]
        [Authorize]
        public IHttpActionResult ChangeBook(int id, Book changedBook)
        {
            var book = this.Data.Books
                .First(b => b.Id == id);
            if (book == null)
            {
                return this.NotFound();
            }

            book.Id = changedBook.Id == 0 ? book.Id : changedBook.Id;
            book.Copies = changedBook.Copies == 0 ? book.Copies : changedBook.Copies;
            book.Price = changedBook.Price == 0 ? book.Price : changedBook.Price;
            book.ReleaseDate = changedBook.ReleaseDate ?? book.ReleaseDate;
            book.Title = changedBook.Title == String.Empty ? book.Title : changedBook.Title;
            book.AgeRestriction = changedBook.AgeRestriction;
            book.Edition = changedBook.Edition;
            book.Author = changedBook.Author ?? book.Author;
            book.Categories = changedBook.Categories ?? book.Categories;
            book.Description = changedBook.Description == String.Empty ? book.Description : changedBook.Description;
            book.RelatedBooks = changedBook.RelatedBooks ?? book.RelatedBooks;


            this.Data.SaveChanges();
            return this.Ok("successful change");
        }

        //DELETE /api/Books/{id}
        [Authorize]
        public IHttpActionResult DeleteBook(int id)
        {
            try
            {
                var book = this.Data.Books
                    .First(b => b.Id == id);
                this.Data.Books.Remove(book);
                this.Data.SaveChanges();
                return this.Ok("deleted successfully");
            }
            catch (Exception)
            {
                return this.NotFound();
            }
        }

        // POST	/api/books 
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddBook(BookBindingModel model)
        {
            if (model == null)
            {
                this.ModelState.AddModelError("model", "The model cannot be null - (request is empty).");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var categories = model.Categories.Split(' ');
            var categoryList = new List<Category>();

            if (categories.Count() != 0)
            {

                foreach (var categoryName in categories)
                {
                    Category category = new Category()
                    {
                        Name = categoryName
                    };
                    categoryList.Add(category);
                }
            }

            var book = new Book()
            {
                Title = model.Title,
                Description = model.Description ?? null,
                Edition = model.Edition,
                Price = model.Price,
                AgeRestriction = model.Restriction,
                Copies = model.Copies,
                ReleaseDate = DateTime.Now,
                Author = this.Data.Authors
                    .First(a => a.Id == model.AuthorId) ?? null,
                RelatedBooks = this.Data.Books
                    .Where(b => b.Id == model.BookId)
                    .ToList()
                    .Count == 0 ? null : this.Data.Books.Where(b => b.Id == model.BookId).ToList(),
                Categories = categoryList ?? null

            };

            this.Data.Books.Add(book);
            this.Data.SaveChanges();
            return this.Ok("created book  with id = " + book.Id);
        }
    }
}