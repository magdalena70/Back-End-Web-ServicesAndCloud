namespace BidSystem.Data.Models
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public User()
        {
            this.Bids = new HashSet<Bid>();
            this.Offers = new HashSet<Offer>();
        }

        public virtual ICollection<Bid> Bids { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
