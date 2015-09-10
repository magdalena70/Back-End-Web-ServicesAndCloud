using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidSystem.RestServices.Models.BindingModels
{
    public class PostOfferBindigModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal InitialPrice { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}