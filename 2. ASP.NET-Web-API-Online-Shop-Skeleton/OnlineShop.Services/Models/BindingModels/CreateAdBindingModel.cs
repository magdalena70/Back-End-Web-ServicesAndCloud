using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Services.Models.BindingModels
{
    public class CreateAdBindingModel
    {
        [Required]
        [StringLength(100), MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IEnumerable<int> Categories { get; set; }
    }
}