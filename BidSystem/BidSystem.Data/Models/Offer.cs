namespace BidSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Offer
    {
        public Offer()
        {
            this.Bids = new HashSet<Bid>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description  { get; set; }

        [Required]
        public decimal InitialPrice { get; set; }

        [Required]
        public DateTime  DateCreated { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public string SellerId { get; set; }

        [Required]
        public virtual User Seller  { get; set; }

        public virtual ICollection<Bid> Bids { get; set; }
    }
}
