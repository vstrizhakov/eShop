using Microsoft.Extensions.Options;

namespace eShop.Common
{
    internal class PublicUriBuilder : IPublicUriBuilder
    {
        private readonly PublicUriConfiguration _configuration;

        public PublicUriBuilder(IOptions<PublicUriConfiguration> configuration)
        {
            _configuration = configuration.Value;
        }

        public string BackendPath(string relativePath)
        {
            return Path(_configuration.Backend, relativePath);
        }

        public string FrontendPath(string relativePath)
        {
            return Path(_configuration.Frontend, relativePath);
        }

        private static string Path(Uri host, string relativePath)
        {
            var uri = new Uri(host, relativePath);
            return uri.ToString();
        }
    }
}
