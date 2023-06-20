using eShop.Configurations;
using Microsoft.Extensions.Options;

namespace eShop.Services
{
    public class PublicUriBuilder : IPublicUriBuilder
    {
        private readonly ApplicationConfiguration _configuration;

        public PublicUriBuilder(IOptions<ApplicationConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public string Path(string relativePath)
        {
            var uri = new Uri(_configuration.Host, relativePath);
            return uri.ToString();
        }
    }
}
