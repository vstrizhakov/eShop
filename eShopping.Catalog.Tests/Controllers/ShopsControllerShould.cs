using AutoMapper;
using eShopping.Catalog.Controllers;
using eShopping.Catalog.Entities;
using eShopping.Catalog.Services;
using Microsoft.AspNetCore.Mvc;

namespace eShopping.Catalog.Tests.Controllers
{
    public class ShopsControllerShould
    {
        [Fact]
        public async Task GetShops()
        {
            // Arrange

            var shops = new[]
            {
                new Shop
                {
                    Name = "Nike",
                },
                new Shop
                {
                    Name = "Puma",
                },
                new Shop
                {
                    Name = "Adidas",
                },
            };

            var shopsService = new Mock<IShopService>();
            shopsService
                .Setup(e => e.GetShopsAsync())
                .ReturnsAsync(shops);

            var expectedResponse = Array.Empty<Models.Shops.Shop>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Models.Shops.Shop>>(shops))
                .Returns(expectedResponse);

            var sut = new ShopsController(shopsService.Object, mapper.Object);

            // Act

            var result = await sut.GetShops();

            // Assert

            shopsService.VerifyAll();
            mapper.VerifyAll();

            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse, (result.Result as OkObjectResult).Value);
        }
    }
}
