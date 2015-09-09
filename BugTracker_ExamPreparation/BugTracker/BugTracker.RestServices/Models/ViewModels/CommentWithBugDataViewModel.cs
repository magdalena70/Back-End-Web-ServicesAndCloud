using BugTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class CommentWithBugDataViewModel
    {
        public static Expression<Func<Comment, CommentWithBugDataViewModel>> Create
        {
            get
            {
                return c => new CommentWithBugDataViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author != null ? c.Author.UserName : null,
                    DateCreated = c.DateCreated,
                    BugId = c.BugId,
                    BugTitle = c.Bug.Title
                };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }

        public int BugId { get; set; }

        public string BugTitle { get; set; }
    }
}