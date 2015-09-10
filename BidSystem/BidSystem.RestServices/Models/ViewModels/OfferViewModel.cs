using BidSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BidSystem.RestServices.Models.ViewModels
{
    public class OfferViewModel
    {
        public static Expression<Func<Offer, OfferViewModel>> Create
        {
            get
            {
                return o => new OfferViewModel()
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Seller = o.Seller.UserName,
                    DatePublished = o.DateCreated,
                    InitialPrice = o.InitialPrice,
                    ExpirationDateTime = o.ExpirationDate,
                    IsExpired = o.ExpirationDate <= DateTime.Now,
                    BidsCount = o.Bids.Count,
                    BidWinner = o.ExpirationDate <= DateTime.Now && o.Bids.Count > 0 ?
                        o.Bids.OrderByDescending(b => b.Price)
                        .FirstOrDefault().Bidder.UserName :
                        null
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Seller { get; set; }

        public DateTime DatePublished { get; set; }

        public decimal InitialPrice { get; set; }

        public DateTime ExpirationDateTime { get; set; }

        public bool IsExpired { get; set; }

        public int BidsCount { get; set; }

        public string BidWinner { get; set; }
    }
}