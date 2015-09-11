namespace OnlineShop.Tests
{
    using Moq;
    using OnlineShop.Data.Repositories;
    using OnlineShop.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    class MockContainer
    {
        public Mock<IRepository<Ad>> AdRepositoryMock { get; set; }

        public Mock<IRepository<Category>> CategoryRepositoryMock { get; set; }

        public Mock<IRepository<ApplicationUser>> UserRepositoryMock { get; set; }

        public Mock<IRepository<AdType>> AdTypeRepositoryMock { get; set; }

        public void PrepareMocks()
        {
            this.SetupFakeCategories();
            this.SetupFakeUsers();
            this.SetupFakeAds();
            this.SetupFakeAdTypes();
        }

        private void SetupFakeAdTypes()
        {
            var fakeAdTypes = new List<AdType>()
            {
                new AdType() { Id = 1, Name="Normal", Index = 100 },
                new AdType() { Id = 2, Name="Premium", Index = 200 },
                new AdType() { Id = 3, Name="Gold", Index = 300 },
                new AdType() { Id = 4, Name="Diamond", Index = 400 }
            };

            this.AdTypeRepositoryMock = new Mock<IRepository<AdType>>();
            this.AdTypeRepositoryMock.Setup(r => r.All())
                .Returns(fakeAdTypes.AsQueryable());

            this.AdTypeRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var adType = fakeAdTypes.FirstOrDefault(a => a.Id == id);
                    return adType;
                });
        }

        private void SetupFakeAds()
        {
            var adTypes = new List<AdType>()
            {
                new AdType() { Name="Normal", Index = 100 },
                new AdType() { Name="Premium", Index = 200 }
            };

            var fakeAds = new List<Ad>()
            {
                new Ad()
                {
                    Id = 5,
                    Name = "Audi A6",
                    Type = adTypes[0],
                    PostedOn = DateTime.Now.AddDays(-6),
                    OwnerId = "123", 
                    Owner = new ApplicationUser() { UserName = "gosho", Id = "123"},
                    Price = 400
                }
            };

            this.AdRepositoryMock = new Mock<IRepository<Ad>>();
            this.AdRepositoryMock.Setup(r => r.All())
                .Returns(fakeAds.AsQueryable());

            this.AdRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var ad = fakeAds.FirstOrDefault(a => a.Id == id);
                    return ad;
                });
        }

        private void SetupFakeUsers()
        {
            var fakeUsers = new List<ApplicationUser>()
            {
                new ApplicationUser() {UserName = "vanko1", Email = "vanko1@abv.bg"},
                new ApplicationUser() {UserName = "ivan", Email = "ivan@abv.bg"},
                new ApplicationUser() {UserName = "stefan", Email = "stefan@abv.bg"},
                new ApplicationUser() {UserName = "petar", Email = "petar@abv.bg"}
            };

            this.UserRepositoryMock = new Mock<IRepository<ApplicationUser>>();
            this.UserRepositoryMock.Setup(r => r.All())
                .Returns(fakeUsers.AsQueryable());

            this.UserRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((string id) =>
                {
                    var user = fakeUsers.FirstOrDefault(u => u.Id == id);
                    return user;
                });
        }

        private void SetupFakeCategories()
        {
            var fakeCategories = new List<Category>()
            {
                new Category(){Id = 1, Name = "Cars"},
                new Category(){Id = 2, Name = "Alcohol"},
                new Category(){Id = 3, Name = "Games"},
                new Category(){Id = 4, Name = "Fun"}
            };

            this.CategoryRepositoryMock = new Mock<IRepository<Category>>();
            this.CategoryRepositoryMock.Setup(r => r.All())
                .Returns(fakeCategories.AsQueryable());

            this.CategoryRepositoryMock.Setup(r => r.Find(It.IsAny<int>()))
                .Returns((int id) =>
                {
                    var category = fakeCategories.FirstOrDefault(c => c.Id == id);
                    return category;
                });
        }
    }
}
