using BookShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BookShopSystem.Models;
using BookShopSystem.Services.Models.ViewModels;

namespace BookShopSystem.Services.Controllers
{
    public class PurchasesController : BaseApiController
    {
        //PUT /api/Books/buy/{id}
        [Route("api/Books/buy/{id}")]
        [Authorize]
        public IHttpActionResult CreatePurchase(int id)
        {
            var book = this.Data.Books.Find(id);
            var currentUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users
                .First(u => u.Id == currentUserId);

            if (book == null)
            {
                return this.BadRequest("invalid book id");
            }

            var purchase = new Purchase()
            {
                Book = book,
                User = user,
                Price = book.Price,
                DateOfPurchase = DateTime.Now,
                IsRecalled = false
            };

            if (book.Copies == 0)
            {
                return this.BadRequest("no book's copies");
            }

            book.Copies = book.Copies - 1;
            this.Data.Purchases.Add(purchase);
            this.Data.SaveChanges();

            var purchaseResult = new
            {
                Price = purchase.Price,
                PurchaseDate = purchase.DateOfPurchase,
                IsRecalled = purchase.IsRecalled,
                PurchasePrice = purchase.Price,
                BookTitle = purchase.Book.Title
            };

            return this.Ok(purchaseResult);
        }

        // GET /api/User/{username}/Purchases
        [Route("api/User/{username}/Purchases")]
        [Authorize]
        public IHttpActionResult GetsAppPurchaseData(string username)
        {
            var searchedUser = this.Data.Users
                .FirstOrDefault(u => u.UserName == username);

            if (searchedUser == null)
            {
                return this.NotFound();
            }

            var userName = searchedUser.UserName;
            var purchases = searchedUser.Purchases.Select(p => new PurchaseViewModel
            {
                PurchasePrice = p.Price,
                BookTitle = p.Book.Title,
                BookPrice = p.Book.Price,
                DateOfPurchase = p.DateOfPurchase,
                IsRecalled = p.IsRecalled
            });

            var purchaseResult = new
            {
                UserName = username,
                Purchases = purchases
            };

            return this.Ok(purchaseResult);
        }
    }
}