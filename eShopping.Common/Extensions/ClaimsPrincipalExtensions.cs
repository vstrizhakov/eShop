using System.Security.Claims;

namespace eShopping.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetAccountId(this ClaimsPrincipal claimsPrincipal)
        {
            Guid? result = null;
            var claim = claimsPrincipal.FindFirst("account_id");
            if (claim != null)
            {
                if (Guid.TryParse(claim.Value, out var guid))
                {
                    result = guid;
                }
            }

            return result;
        }
    }
}