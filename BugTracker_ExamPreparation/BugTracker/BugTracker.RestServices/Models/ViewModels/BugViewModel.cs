using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Data.Models;
using System.Linq.Expressions;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class BugViewModel
    {
        public static Expression<Func<Bug, BugViewModel>> Create
        {
            get
            {
                return b => new BugViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Status = b.Status.ToString(),
                    Author = b.Author != null ? b.Author.UserName : null,
                    DateCreated = b.DateCreated
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Status { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }
    }
}