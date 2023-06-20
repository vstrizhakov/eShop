using Microsoft.AspNetCore.Mvc;

namespace eShop.Extensions
{
    public static class IUrlHelperExtensions
    {
        public static string Path(this IUrlHelper helper, string relativePath)
        {
            var request = helper.ActionContext.HttpContext.Request;
            var urlBuilder = new UriBuilder(request.Scheme);
            var host = request.Host;
            urlBuilder.Host = host.Host;
            if (host.Port.HasValue)
            {
                urlBuilder.Port = host.Port.Value;
            }
            urlBuilder.Path = relativePath;
            return urlBuilder.ToString();
        }
    }
}
