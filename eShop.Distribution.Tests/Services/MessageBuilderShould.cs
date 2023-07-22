using eShop.Distribution.Services;
using eShop.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Distribution.Tests.Services
{
    public class MessageBuilderShould
    {
        [Fact]
        public void BuildMessageFromComposition()
        {
            // Arrange

            var composition = new Composition
            {
                Images = new List<Uri>
                {
                    new Uri("https://main.image.com"),
                    new Uri("https://first.image.com"),
                },
                Products = new List<Product>
                {
                    new Product
                    {
                        Name = "Product 1",
                        Price = 12.5,
                        Url = new Uri("https://product1.com"),
                    },
                    new Product
                    {
                        Name = "Product 2",
                        Price = 52.1,
                        Url = new Uri("https://product2.com"),
                    },
                },
            };

            var sut = new MessageBuilder();

            // Act

            var result = sut.FromComposition(composition);

            // Assert

            Assert.Equal("https://main.image.com/", result.Image.ToString());
            Assert.Equal("Product 1 - 12.5\n\nProduct 2 - 52.1", result.Caption);
        }
    }
}
