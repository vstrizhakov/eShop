using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using eShop.Identity.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace eShop.Identity.Services
{
    public class ProfileService : DefaultProfileService
    {
        private readonly UserManager<User> _userManager;

        public ProfileService(
            UserManager<User> userManager,
            ILogger<DefaultProfileService> logger)
            : base(logger)
        {
            _userManager = userManager;
        }

        public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            await base.GetProfileDataAsync(context);

            var principal = context.Subject;
            var user = await _userManager.GetUserAsync(principal);
            if (user != null)
            {
                var claims = new List<Claim>();
                var requestedClaims = context.RequestedClaimTypes;


                var accountIdClaimType = "account_id";
                if (requestedClaims.Contains(accountIdClaimType))
                {
                    var accountId = user.AccountId;
                    if (accountId.HasValue)
                    {
                        claims.Add(new Claim(accountIdClaimType, accountId.Value.ToString()));
                    }
                }

                if (requestedClaims.Contains(JwtClaimTypes.FamilyName))
                {
                    if (user.LastName != null)
                    {
                        claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                    }
                }

                if (requestedClaims.Contains(JwtClaimTypes.GivenName))
                {
                    claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                }

                context.AddRequestedClaims(claims);
            }
        }
    }
}
