using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace OnlineShop.Services.Models.ViewModels
{
    public class AdViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public OwnerViewModel Owner { get; set; }

        public string Type { get; set; }

        public DateTime PostedOn { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public static Expression<Func<Ad, AdViewModel>> Create
        {
            get
            {
                return ad => new AdViewModel
                {
                    Id = ad.Id,
                    Name = ad.Name,
                    Description = ad.Description,
                    Price = ad.Price,
                    Owner = new OwnerViewModel
                    {
                        Id = ad.Owner.Id,
                        Username = ad.Owner.UserName
                    },
                    Type = ad.Type.Name,
                    PostedOn = ad.PostedOn,
                    Categories = ad.Categories.Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    })
                };
            }
        }
    }
}