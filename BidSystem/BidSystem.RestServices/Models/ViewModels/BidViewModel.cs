using BidSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BidSystem.RestServices.Models.ViewModels
{
    public class BidViewModel
    {
        public static Expression<Func<Bid, BidViewModel>> Create
        {
            get
            {
                return b => new BidViewModel()
                {
                    Id = b.Id,
                    OfferId = b.OfferId,
                    DateCreated = b.DateCreated,
                    Bidder = b.Bidder.UserName,
                    OfferedPrice = b.Price,
                    Comment = b.Comment
                };
            }
        }

        public int Id { get; set; }

        public int OfferId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Bidder { get; set; }

        public decimal OfferedPrice { get; set; }

        public string Comment { get; set; }
    }
}