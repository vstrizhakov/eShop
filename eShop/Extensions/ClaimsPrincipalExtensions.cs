using System.Security.Claims;

namespace eShop.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetSub(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue("sub");
        }
    }
}
