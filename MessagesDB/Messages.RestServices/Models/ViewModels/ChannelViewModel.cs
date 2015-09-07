using Messages.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Messages.RestServices.Models.ViewModels
{
    public class ChannelViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<Channel, ChannelViewModel>> Create
        {
            get
            {
                return ch => new ChannelViewModel
                {
                    Id = ch.Id,
                    Name = ch.Name
                };
            }
        }
    }
}