using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using BidSystem.RestServices.Models.ViewModels;
using BidSystem.RestServices.Models.BindingModels;

namespace BidSystem.RestServices.Controllers
{
    [RoutePrefix("api/bids")]
    public class BidsController : BaseApiController
    {
        //GET /api/bids/my
        [HttpGet]
        [Route("my")]
        [Authorize]
        public IHttpActionResult ListUserBids()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var bids = this.Data.Bids
                .Where(b => b.BidderId == currentUserId)
                .OrderByDescending(b => b.DateCreated)
                .ThenByDescending(b => b.Id)
                .Select(BidViewModel.Create);

            return this.Ok(bids);
        }

        //GET /api/bids/won
        [HttpGet]
        [Route("won")]
        [Authorize]
        public IHttpActionResult ListUserWonBids()
        {
            var currentUserId = this.User.Identity.GetUserId();
            var currentUserName = this.User.Identity.GetUserName();

            var offer = this.Data.Offers
                .Where(o => o.ExpirationDate <= DateTime.Now && o.Bids.Count > 0 &&
                        o.Bids.OrderByDescending(b => b.Price)
                        .FirstOrDefault().Bidder.UserName == currentUserName)
                .OrderByDescending(o => o.DateCreated)
                .ThenByDescending(o => o.Id)
                .Select(o => o.Bids
                    .Where(b => b.BidderId == currentUserId)
                    .Select(b => new BidViewModel() 
                    {
                        Id = b.Id,
                        OfferId = b.OfferId,
                        DateCreated = b.DateCreated,
                        Bidder = b.Bidder.UserName,
                        OfferedPrice = b.Price,
                        Comment = b.Comment
                    })
                );

            return this.Ok(offer);
        }
    }
}