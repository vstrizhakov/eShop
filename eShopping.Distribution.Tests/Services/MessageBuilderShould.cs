using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopping.Distribution.Tests.Services
{
    public class MessageBuilderShould
    {
        [Fact]
        public void BuildMessageFromComposition()
        {
            // Arrange

            var composition = new Announce
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

            var result = sut.FromComposition(composition, new Entities.DistributionSettings());

            // Assert

            Assert.Equal("https://main.image.com/", result.Image.ToString());
            Assert.Equal("Product 1 - 12.5\n\nProduct 2 - 52.1", result.Caption);
        }
    }
}
