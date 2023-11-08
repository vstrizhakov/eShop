﻿using Duende.IdentityServer.Models;
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

                var accountIdClaimType = "account_id";
                if (context.RequestedClaimTypes.Contains(accountIdClaimType))
                {
                    var accountId = user.AccountId;
                    if (accountId.HasValue)
                    {
                        claims.Add(new Claim(accountIdClaimType, accountId.Value.ToString()));
                    }
                }

                context.AddRequestedClaims(claims);
            }
        }
    }
}
