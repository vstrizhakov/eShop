using AutoMapper;
using Duende.IdentityServer.Services;
using eShop.Identity.Entities;
using eShop.Identity.Models;
using eShop.Messaging;
using eShop.Messaging.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("signUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp(
            [FromBody] SignUpRequest request,
            [FromServices] UserManager<User> userManager,
            [FromServices] IProducer producer,
            [FromServices] IMapper mapper)
        {
            var succeeded = false;

            if (!User.Identity.IsAuthenticated)
            {
                var user = mapper.Map<User>(request);
                var result = await userManager.CreateAsync(user, request.Password);

                succeeded = result.Succeeded;

                if (succeeded)
                {
                    var message = new IdentityUserCreateAccountRequestMessage
                    {
                        IdentityUserId = user.Id,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email,
                        PhoneNumber = request.PhoneNumber,
                    };
                    producer.Publish(message);
                }
            }

            var response = new SignUpResponse
            {
                Succeeded = succeeded,
            };
            return Ok(response);
        }


        [HttpPost("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(
            [FromBody] SignInRequest request,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] SignInManager<User> signInManager,
            [FromServices] IServerUrls serverUrls)
        {
            var response = new SignInResponse();
            var succeeded = false;

            if (!User.Identity.IsAuthenticated)
            {
                var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, request.Remember, false);
                succeeded = result.Succeeded;
            }

            response.Succeeded = succeeded;
            if (succeeded)
            {
                // TODO: Check request.ReturnUrl is null
                var url = request.ReturnUrl;
                var context = await interaction.GetAuthorizationContextAsync(url);
                if (context != null)
                {
                    response.ValidReturnUrl = url;
                }
                else
                {
                    response.ValidReturnUrl = serverUrls.BaseUrl;
                }
            }

            return Ok(response);
        }

        [HttpGet("signOut")]
        [Authorize]
        public async Task<ActionResult<SignOutInfo>> SignOut(
            [FromQuery] string logoutId,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] SignInManager<User> signInManager)
        {
            var logoutInfo = await interaction.GetLogoutContextAsync(logoutId);

            var showSignoutPrompt = logoutInfo?.ShowSignoutPrompt;

            PostSignOutInfo? postSignOutInfo = null;
            if (showSignoutPrompt != true)
            {
                if (User.Identity.IsAuthenticated)
                {
                    await signInManager.SignOutAsync();
                }

                postSignOutInfo = new PostSignOutInfo
                {
                    IFrameUrl = logoutInfo?.SignOutIFrameUrl,
                    RedirectUrl = logoutInfo?.PostLogoutRedirectUri,
                };
            }

            return Ok(new SignOutInfo
            {
                Prompt = showSignoutPrompt,
                PostInfo = postSignOutInfo,
            });
        }

        [HttpPost("signOut")]
        [Authorize]
        public async Task<ActionResult<SignOutInfo>> PostSignOut(
            [FromQuery] string logoutId,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] SignInManager<User> signInManager)
        {
            var logoutInfo = await interaction.GetLogoutContextAsync(logoutId);

            await signInManager.SignOutAsync();

            return Ok(new SignOutInfo
            {
                PostInfo = new PostSignOutInfo
                {
                    IFrameUrl = logoutInfo?.SignOutIFrameUrl,
                    RedirectUrl = logoutInfo?.PostLogoutRedirectUri,
                },
            });
        }
    }
}