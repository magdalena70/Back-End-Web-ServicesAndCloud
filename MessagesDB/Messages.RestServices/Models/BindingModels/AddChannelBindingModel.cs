using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Messages.RestServices.Models.BindingModels
{
    public class AddChannelBindingModel
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be in range [1 ... 100] characters.")]
        public string Name { get; set; }
    }
}