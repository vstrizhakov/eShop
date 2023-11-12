using AutoMapper;
using Duende.IdentityServer.Services;
using eShop.Bots.Links;
using eShop.Identity.Entities;
using eShop.Identity.Models;
using eShop.Identity.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("signUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp(
            [FromBody] SignUpRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserManager<User> userManager,
            [FromServices] IMapper mapper)
        {
            var succeeded = false;

            if (!User.Identity.IsAuthenticated)
            {
                var user = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
                if (user == null)
                {
                    user = mapper.Map<User>(request);
                    var result = await userManager.CreateAsync(user, request.Password);

                    succeeded = result.Succeeded;
                }

                if (succeeded)
                {
                    await HttpContext.SignInAsync("PhoneNumberConfirmationCookie", BuildUserPrincipal(user));
                }
            }

            var response = new SignUpResponse
            {
                Succeeded = succeeded,
            };
            return Ok(response);
        }

        [HttpPost("checkConfirmation")]
        public async Task<ActionResult<CheckConfirmationResponse>> GetSignUpConfirmation(
            [FromBody] CheckConfirmationRequest request,
            [FromServices] UserManager<User> userManager,
            [FromServices] SignInManager<User> signInManager,
            [FromServices] ITelegramLinkGenerator telegramLinkGenerator,
            [FromServices] IViberLinkGenerator viberLinkGenerator,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] IServerUrls serverUrls)
        {
            var result = await HttpContext.AuthenticateAsync("PhoneNumberConfirmationCookie");
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var response = new CheckConfirmationResponse
            {
                Confirmed = false,
            };

            var principal = result.Principal;
            if (principal != null)
            {
                var userId = principal.FindFirstValue("user_id")!;
                if (userId != null)
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        response.Confirmed = user.PhoneNumberConfirmed;

                        if (!response.Confirmed)
                        {
                            response.Links = new ConfirmationLinks
                            {
                                Telegram = telegramLinkGenerator.Generate("cpn"),
                                Viber = viberLinkGenerator.Generate("cpn"),
                            };
                        }
                        else
                        {
                            // TODO: implement persistent
                            await signInManager.SignInAsync(user, false);

                            await HttpContext.SignOutAsync("PhoneNumberConfirmationCookie");

                            response.ValidReturnUrl = await GetValidReturnUrlAsync(interaction, serverUrls, request.ReturnUrl);
                        }
                    }
                }
            }

            return Ok(response);
        }

        [HttpGet("signIn")]
        public async Task<ActionResult<SignInInfo>> SignIn()
        {
            var response = new SignInInfo();

            if (!User.Identity.IsAuthenticated)
            {
                var result = await HttpContext.AuthenticateAsync("PhoneNumberConfirmationCookie");
                if (result.Succeeded)
                {
                    response.WaitingForConfirmation = true;
                }
            }

            return Ok(response);
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn(
            [FromBody] SignInRequest request,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserManager<User> userManager,
            [FromServices] SignInManager<User> signInManager,
            [FromServices] IServerUrls serverUrls)
        {
            var response = new SignInResponse();

            if (!User.Identity.IsAuthenticated)
            {
                var user = await userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
                if (user != null)
                {
                    var password = request.Password;
                    var isValidCrendetials = await userManager.CheckPasswordAsync(user, password);
                    if (isValidCrendetials)
                    {
                        if (user.PhoneNumberConfirmed)
                        {
                            var result = await signInManager.PasswordSignInAsync(user, request.Password, request.Remember, false);
                            response.Succeeded = result.Succeeded;
                        }
                        else
                        {
                            response.ConfirmationRequired = true;

                            await HttpContext.SignInAsync("PhoneNumberConfirmationCookie", BuildUserPrincipal(user));
                        }
                    }
                }
            }
            else
            {
                response.Succeeded = true;
            }

            if (response.Succeeded)
            {
                response.ValidReturnUrl = await GetValidReturnUrlAsync(interaction, serverUrls, request.ReturnUrl);
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

        private ClaimsPrincipal BuildUserPrincipal(User user)
        {
            var identity = new ClaimsIdentity("PhoneNumberConfirmationCookie");
            identity.AddClaim(new Claim("user_id", user.Id));

            return new ClaimsPrincipal(identity);
        }

        private async Task<string> GetValidReturnUrlAsync(IIdentityServerInteractionService interaction, IServerUrls serverUrls, string? returnUrl)
        {
            string validReturnUrl;
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            if (context != null)
            {
                validReturnUrl = returnUrl!;
            }
            else
            {
                validReturnUrl = serverUrls.BaseUrl;
            }

            return validReturnUrl;
        }
    }
}