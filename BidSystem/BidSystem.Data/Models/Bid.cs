
namespace BidSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Bid
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime DateCreated  { get; set; }

        public string Comment  { get; set; }

        public string BidderId  { get; set; }

        [Required]
        public User Bidder  { get; set; }

        public int OfferId { get; set; }

        [Required]
        public Offer Offer { get; set; }
    }
}
