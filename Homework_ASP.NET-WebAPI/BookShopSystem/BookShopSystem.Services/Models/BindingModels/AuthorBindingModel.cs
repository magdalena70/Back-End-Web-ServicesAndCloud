using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookShopSystem.Services.Models.BindingModels
{
    public class AuthorBindingModel
    {

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
    }
}