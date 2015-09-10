using BidSystem.RestServices.Models.ViewModels;
using BidSystem.RestServices.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BidSystem.Data.Models;

namespace BidSystem.RestServices.Controllers
{
    [RoutePrefix("api/offers")]
    public class OffersController : BaseApiController
    {
        //GET /api/offers/all
        [HttpGet]
        [Route("all")]
        public IHttpActionResult ListAllOffers()
        {
            var offers = this.Data.Offers
                .OrderByDescending(o => o.DateCreated)
                .ThenByDescending(o => o.Id)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        //GET /api/offers/expired
        [HttpGet]
        [Route("expired")]
        public IHttpActionResult ListExpiredOffers()
        {
            var expiredOffers = this.Data.Offers
                .Where(o => o.ExpirationDate <= DateTime.Now)
                .OrderByDescending(o => o.ExpirationDate)
                .ThenByDescending(o => o.Id)
                .Select(OfferViewModel.Create);

            return this.Ok(expiredOffers);
        }

        //GET /api/offers/active
        [HttpGet]
        [Route("active")]
        public IHttpActionResult ListActiveOffers()
        {
            var activeOffers = this.Data.Offers
                .Where(o => o.ExpirationDate > DateTime.Now)
                .OrderByDescending(o => o.ExpirationDate)
                .ThenByDescending(o => o.Id)
                .Select(OfferViewModel.Create);

            return this.Ok(activeOffers);
        }

        //GET /api/offers/details/{id}
        [HttpGet]
        [Route("details/{id}")]
        public IHttpActionResult GetOfferDetailsById(int id)
        {
            var offer = this.Data.Offers
                .Where(o => o.Id == id)
                .Select(OfferDetailsViewModel.Create)
                .FirstOrDefault();
            if (offer == null)
            {
                return this.NotFound();
            }

            return this.Ok(offer);
        }

        //GET /api/offers/my
        [HttpGet]
        [Route("my")]
        [Authorize]
        public IHttpActionResult ListUserOffers()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var offers = this.Data.Offers
                .Where(o => o.SellerId == currentUserId)
                .OrderByDescending(o => o.DateCreated)
                .ThenByDescending(o => o.Id)
                .Select(OfferViewModel.Create);

            return this.Ok(offers);
        }

        //POST /api/offers
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostOffer(PostOfferBindigModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Title, InitialPrice and ExpirationDate cannot be empty");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var offer = new Offer()
            {
                Title = model.Title,
                Description = model.Description,
                InitialPrice = model.InitialPrice,
                DateCreated = DateTime.Now,
                ExpirationDate = model.ExpirationDate,
                SellerId = currentUserId
            };

            this.Data.Offers.Add(offer);
            this.Data.SaveChanges();

            return this.CreatedAtRoute(
                    "DefaultApi",
                    new { id = offer.Id },
                    new
                    {
                        offer.Id,
                        Seller = this.User.Identity.GetUserName(),
                        Message = "Offer created."
                    });
        }

        //POST /api/offers/117/bid
        [HttpPost]
        [Route("{id}/bid")]
        [Authorize]
        public IHttpActionResult PostBidByOfferId(int id, PostBidBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Data cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var offer = this.Data.Offers
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    Id = o.Id,
                    InitialPrice = o.InitialPrice,
                    ExpirationDate = o.ExpirationDate,
                    Bids = o.Bids.Select(b => b.Price)
                })
                .FirstOrDefault();

            if (offer == null)
            {
                return this.NotFound();
            }

            if (offer.ExpirationDate < DateTime.Now)
            {
                return this.BadRequest("Offer has expired.");
            }

            var currentMaxBidPrice = offer.InitialPrice;
            if (offer.Bids.Any())
            {
                currentMaxBidPrice = offer.Bids.Max();
            }

            if (model.BidPrice <= currentMaxBidPrice)
            {
                return this.BadRequest("Your bid should be > " + currentMaxBidPrice);
            }

            if (!this.User.Identity.IsAuthenticated)
            {
                return this.Unauthorized();
            }

            var currentUserId = this.User.Identity.GetUserId();
            var bid = new Bid()
            {
                BidderId = currentUserId,
                Price =  model.BidPrice,
                Comment = model.Comment,
                DateCreated = DateTime.Now,
                OfferId = offer.Id

            };

            this.Data.Bids.Add(bid);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                Id = bid.Id,
                Bidder = this.User.Identity.GetUserName(),
                Message = "Bid created."
            });
        }
    }
}