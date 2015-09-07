using Messages.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Messages.RestServices.Models.ViewModels
{
    public class UserMessageViewModel
    {
        public static Expression<Func<UserMessage, UserMessageViewModel>> Create
        {
            get
            {
                return um => new UserMessageViewModel
                {
                    Id = um.Id,
                    Text = um.Text,
                    DateSent = um.DateSent,
                    Sender = um.Sender.UserName
                };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }
    }
}