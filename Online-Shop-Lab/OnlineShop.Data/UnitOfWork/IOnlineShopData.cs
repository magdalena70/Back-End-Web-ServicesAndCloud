
namespace OnlineShop.Data.UnitOfWork
{
    using OnlineShop.Models;
    using Repositories;

    public interface IOnlineShopData
    {
        IRepository<Ad> Ads { get; }

        IRepository<AdType> AdTypes { get; }

        IRepository<Category> Categories { get; }

        IRepository<ApplicationUser> Users { get; }

        int SaveChanges();
    }
}
