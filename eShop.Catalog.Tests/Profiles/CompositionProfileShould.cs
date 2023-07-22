using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Products;
using eShop.Catalog.Profiles;

namespace eShop.Catalog.Tests.Profiles
{
    public class CompositionProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CompositionProfile>();

                config.CreateMap<CreateProductRequest, Product>()
                    .ForAllMembers(options => options.Ignore());
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
