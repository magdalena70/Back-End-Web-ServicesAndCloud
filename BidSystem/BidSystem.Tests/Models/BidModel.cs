namespace BidSystem.Tests.Models
{
    using System;

    public class BidModel
    {
        public int? Id { get; set; }

        public decimal? BidPrice { get; set; }

        public string Comment { get; set; }
    }
}
