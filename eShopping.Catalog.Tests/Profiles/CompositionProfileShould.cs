using AutoMapper;
using eShopping.Catalog.Entities;
using eShopping.Catalog.Models.Products;
using eShopping.Catalog.Profiles;

namespace eShopping.Catalog.Tests.Profiles
{
    public class CompositionProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var sut = new MapperConfiguration(config =>
            {
                config.AddProfile<AnnounceProfile>();

                config.CreateMap<CreateProductRequest, Product>()
                    .ForAllMembers(options => options.Ignore());
            });

            // Act & Assert

            sut.AssertConfigurationIsValid();
        }
    }
}
