using AutoMapper;
using Duende.IdentityServer.Services;
using eShop.Bots.Links;
using eShop.Common;
using eShop.Identity.Entities;
using eShop.Identity.Models;
using eShop.Identity.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ResetPassword;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;

namespace eShop.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string PhoneNumberConfirmationCookie = nameof(PhoneNumberConfirmationCookie);
        
        [HttpPost("signUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp(
            [FromBody] SignUpRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserManager<User> userManager,
            [FromServices] IMapper mapper)
        {
            var response = new SignUpResponse();

            if (!User.Identity.IsAuthenticated)
            {
                var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
                if (user == null)
                {
                    user = mapper.Map<User>(request);
                    var result = await userManager.CreateAsync(user, request.Password);

                    response.Succeeded = result.Succeeded;
                    if (!response.Succeeded)
                    {
                        response.ErrorCode = ErrorCode.InvalidPassword;
                    }
                }
                else
                {
                    response.ErrorCode = ErrorCode.UserAlreadyExists;
                }

                if (response.Succeeded)
                {
                    await HttpContext.SignInAsync(PhoneNumberConfirmationCookie, BuildUserPrincipal(user));
                }
            }

            return Ok(response);
        }

        [HttpPost("checkConfirmation")]
        public async Task<ActionResult<CheckConfirmationResponse>> CheckConfirmation(
            [FromBody] CheckConfirmationRequest request,
            [FromServices] UserManager<User> userManager,
            [FromServices] SignInManager<User> signInManager,
            [FromServices] ITelegramLinkGenerator telegramLinkGenerator,
            [FromServices] IViberLinkGenerator viberLinkGenerator,
            [FromServices] IIdentityServerInteractionService interaction,
            [FromServices] IServerUrls serverUrls)
        {
            var result = await HttpContext.AuthenticateAsync(PhoneNumberConfirmationCookie);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var response = new CheckConfirmationResponse
            {
                Confirmed = false,
            };

            var principal = result.Principal;
            if (principal == null)
            {
                await HttpContext.SignOutAsync(PhoneNumberConfirmationCookie);
                return BadRequest();
            }

            var userId = principal.FindFirstValue("user_id");
            if (userId == null)
            {
                await HttpContext.SignOutAsync(PhoneNumberConfirmationCookie);
                return BadRequest();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                await HttpContext.SignOutAsync(PhoneNumberConfirmationCookie);
                return BadRequest();
            }

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

                await HttpContext.SignOutAsync(PhoneNumberConfirmationCookie);

                response.ValidReturnUrl = await GetValidReturnUrlAsync(interaction, serverUrls, request.ReturnUrl);
            }

            return Ok(response);
        }

        [HttpPost("cancelConfirmation")]
        public async Task<ActionResult> CancelConfirmation()
        {
            await HttpContext.SignOutAsync(PhoneNumberConfirmationCookie);

            return Ok();
        }

        [HttpGet("signIn")]
        public async Task<ActionResult<SignInInfo>> SignIn()
        {
            var response = new SignInInfo();

            if (!User.Identity.IsAuthenticated)
            {
                var result = await HttpContext.AuthenticateAsync(PhoneNumberConfirmationCookie);
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
                var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
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

                            await HttpContext.SignInAsync(PhoneNumberConfirmationCookie, BuildUserPrincipal(user));
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

        [Authorize]
        [HttpGet("signOut")]
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

        [Authorize]
        [HttpPost("signOut")]
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

        [HttpPost("requestPasswordReset")]
        public async Task<ActionResult> RequestPasswordReset(
            [FromBody] RequestPasswordResetRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserManager<User> userManager,
            [FromServices] IProducer producer,
            [FromServices] IPublicUriBuilder publicUriBuilder)
        {
            var response = new RequestPasswordResetResponse();

            var phoneNumber = request.PhoneNumber;
            var user = await userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                response.ErrorCode = ErrorCode.UserNotFound;
                return Ok(response);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var message = new SendResetPasswordMessage
            {
                AccountId = user.AccountId!.Value,
                ResetPasswordLink = publicUriBuilder.Path(QueryHelpers.AddQueryString("/auth/completePasswordReset", new Dictionary<string, string?>
                {
                    { "phoneNumber", phoneNumber },
                    { "token", token },
                })),
            };
            producer.Publish(message);

            response.Succeeded = true;

            return Ok(response);
        }

        [HttpPost("completePasswordReset")]
        public async Task<ActionResult> CompleteResetPassword(
            [FromBody] CompleteResetPasswordRequest request,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserManager<User> userManager)
        {
            var response = new CompleteResetPasswordResponse();

            var user = await userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
            if (user == null)
            {
                response.ErrorCode = ErrorCode.UserNotFound;
                return Ok(response);
            }

            var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                response.ErrorCode = ErrorCode.InvalidPassword;
            }

            response.IsSuccess = result.Succeeded;

            return Ok(response);
        }

        private ClaimsPrincipal BuildUserPrincipal(User user)
        {
            var identity = new ClaimsIdentity(PhoneNumberConfirmationCookie);
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