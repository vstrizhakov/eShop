using AutoMapper;
using eShopping.Distribution.Profiles;

namespace eShopping.Distribution.Tests.Profiles
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
