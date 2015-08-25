using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShopSystem.Services.Models.BindingModels
{
    public class CategoryBindingModel
    {
        [Required]
        public string Name { get; set; }
    }
}