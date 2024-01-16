using eShopping.Catalog.Entities;
using eShopping.Catalog.Repositories;
using eShopping.Catalog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopping.Catalog.Tests.Services
{
    public class ShopServiceShould
    {
        [Fact]
        public async Task GetShops()
        {
            // Arrange

            var expectedResponse = Array.Empty<Shop>();

            var shopRepository = new Mock<IShopRepository>();
            shopRepository
                .Setup(e => e.GetShopsAsync())
                .ReturnsAsync(expectedResponse);

            var sut = new ShopService(shopRepository.Object);

            // Act

            var result = await sut.GetShopsAsync();

            // Assert

            Assert.Equal(expectedResponse, result);
        }
    }
}
