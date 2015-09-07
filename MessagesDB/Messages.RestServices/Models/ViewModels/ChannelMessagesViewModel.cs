using Messages.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Messages.RestServices.Models.ViewModels
{
    public class ChannelMessagesViewModel
    {
        public static Expression<Func<ChannelMessage, ChannelMessagesViewModel>> Create
        {
            get
            {
                return ch => new ChannelMessagesViewModel
                {
                    Id = ch.Id,
                    Text = ch.Text,
                    DateSent = ch.DateSent,
                    Sender = ch.Sender.UserName
                };
            }
        }

        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }
    }
}