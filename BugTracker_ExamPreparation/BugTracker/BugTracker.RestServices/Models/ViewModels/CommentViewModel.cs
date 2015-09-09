using BugTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BugTracker.RestServices.Models.ViewModels
{
    public class CommentViewModel
    {
        public static Expression<Func<Comment, CommentViewModel>> Create
        {
            get
            {
                return c => new CommentViewModel
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author != null ? c.Author.UserName : null,
                    DateCreated = c.DateCreated
                };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public string Author { get; set; }

        public DateTime DateCreated { get; set; }
    }
}