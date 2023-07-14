using AutoMapper;
using Duende.IdentityServer.Services;
using eShop.Identity.Entities;
using eShop.Identity.Models;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.RabbitMq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IServerUrls _serverUrls;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public AuthController(
            IIdentityServerInteractionService interaction,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IServerUrls serverUrls,
            IMapper mapper,
            IProducer producer)
        {
            _interaction = interaction;
            _userManager = userManager;
            _signInManager = signInManager;
            _serverUrls = serverUrls;
            _mapper = mapper;
            _producer = producer;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<SignUpResponse>> SignUp([FromBody] SignUpRequest request)
        {
            var succeeded = false;

            if (!User.Identity.IsAuthenticated)
            {
                var user = _mapper.Map<User>(request);
                var result = await _userManager.CreateAsync(user, request.Password);

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
                    _producer.Publish(message);
                }
            }

            var response = new SignUpResponse
            {
                Succeeded = succeeded,
            };
            return Ok(response);
        }


        [HttpPost("signIn")]
        public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInRequest request)
        {
            var response = new SignInResponse();
            var succeeded = false;

            if (!User.Identity.IsAuthenticated)
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, request.Remember, false);
                succeeded = result.Succeeded;
            }

            response.Succeeded = succeeded;
            if (succeeded)
            {
                var url = request.ReturnUrl;
                var context = await _interaction.GetAuthorizationContextAsync(url);
                if (context != null)
                {
                    response.ValidReturnUrl = url;
                }
                else
                {
                    response.ValidReturnUrl = _serverUrls.BaseUrl;
                }
            }

            return Ok(response);
        }

        [HttpGet("signOut")]
        [Authorize]
        public async Task<ActionResult<SignOutInfo>> SignOut([FromQuery] string logoutId)
        {
            var logoutInfo = await _interaction.GetLogoutContextAsync(logoutId);

            if (logoutInfo != null)
            {
                if (!logoutInfo.ShowSignoutPrompt || !User.Identity.IsAuthenticated)
                {
                    await _signInManager.SignOutAsync();

                    return Ok(new SignOutInfo
                    {
                        PostInfo = new PostSignOutInfo
                        {
                            IFrameUrl = logoutInfo.SignOutIFrameUrl,
                            RedirectUrl = logoutInfo.PostLogoutRedirectUri,
                        },
                    });
                }
            }

            return Ok(new SignOutInfo
            {
                Prompt = User.Identity.IsAuthenticated,
            });
        }

        [HttpPost("signOut")]
        [Authorize]
        public async Task<ActionResult<SignOutInfo>> PostSignOut([FromQuery] string logoutId)
        {
            var logoutInfo = await _interaction.GetLogoutContextAsync(logoutId);

            await _signInManager.SignOutAsync();

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