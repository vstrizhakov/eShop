using AutoMapper;
using eShop.Accounts.Profiles;

namespace eShop.Accounts.Tests.Profiles
{
    public class AccountProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var sut = new MapperConfiguration(options =>
            {
                options.AddProfile<AccountProfile>();
            });

            // Act & Assert

            sut.AssertConfigurationIsValid();
        }
    }
}
