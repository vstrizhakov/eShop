using AutoMapper;
using eShop.Identity.Profiles;

namespace eShop.Identity.Tests.Profiles
{
    public class AuthProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<UserProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
