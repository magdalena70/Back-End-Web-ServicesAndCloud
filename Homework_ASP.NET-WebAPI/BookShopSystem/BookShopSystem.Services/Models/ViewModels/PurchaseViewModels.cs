using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BookShopSystem.Services.Models.ViewModels
{
    public class PurchaseViewModel
    {
        public string BookTitle { get; set; }

        public Decimal BookPrice { get; set; }

        public decimal PurchasePrice { get; set; }

        public DateTime DateOfPurchase { get; set; }

        public bool IsRecalled { get; set; }
    }
}