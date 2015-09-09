using BugTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class BugByIdViewModel
    {
        public static Expression<Func<Bug, BugByIdViewModel>> Create
        {
            get
            {
                return b => new BugByIdViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Status = b.Status.ToString(),
                    Author = b.Author != null ? b.Author.UserName : null,
                    DateCreated = b.DateCreated,
                    Comments = b.Comments
                        .OrderByDescending(c => c.DateCreated)
                        .ThenByDescending(c => c.Id)
                        .Select(c => new CommentViewModel()
                        { 
                            Id = c.Id,
                            Text = c.Text,
                            Author = c.Author != null ? c.Author.UserName : null,
                            DateCreated = c.DateCreated
                        })
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}