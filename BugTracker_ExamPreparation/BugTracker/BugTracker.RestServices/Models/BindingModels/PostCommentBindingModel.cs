using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.RestServices.Models.BindingModels
{
    public class PostCommentBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}