using AutoMapper;
using eShop.Distribution.Profiles;

namespace eShop.Distribution.Tests.Profiles
{
    public class ClientProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ClientProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
