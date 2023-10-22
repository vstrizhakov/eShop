using AutoMapper;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using eShop.Identity.Controllers;
using eShop.Identity.Entities;
using eShop.Identity.Models;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.Identity.Tests.Controllers
{
    public class AuthControllerShould
    {
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<SignInManager<User>> _signInManager;

        public AuthControllerShould()
        {
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _signInManager = new Mock<SignInManager<User>>(_userManager.Object, httpContextAccessor.Object, userClaimsPrincipalFactory.Object, null, null, null, null);
        }

        [Fact]
        [Trait("Category", "SignUp")]
        public async Task Fail_WhenIsAuthenticated_OnSignUp()
        {
            // Arrange

            var request = new SignUpRequest();

            var producer = new Mock<IProducer>();

            var mapper = new Mock<IMapper>();

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(true),
            };

            // Act

            var result = await sut.SignUp(request, _userManager.Object, producer.Object, mapper.Object);

            // Assert

            _userManager.VerifyAll();
            producer.VerifyAll();
            mapper.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignUpResponse>((result.Result as OkObjectResult).Value);
            Assert.False(((result.Result as OkObjectResult).Value as SignUpResponse).Succeeded);
        }

        [Fact]
        [Trait("Category", "SignUp")]
        public async Task Fail_WhenIsNotAuthenticated_OnSignUp()
        {
            // Arrange

            var request = new SignUpRequest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
                Password = "password",
            };

            var user = new User();

            _userManager
                .Setup(e => e.CreateAsync(user, request.Password))
                .ReturnsAsync(IdentityResult.Failed());

            var producer = new Mock<IProducer>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<User>(request))
                .Returns(user);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(false),
            };

            // Act

            var result = await sut.SignUp(request, _userManager.Object, producer.Object, mapper.Object);

            // Assert

            _userManager.VerifyAll();
            producer.VerifyAll();
            mapper.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignUpResponse>((result.Result as OkObjectResult).Value);
            Assert.False(((result.Result as OkObjectResult).Value as SignUpResponse).Succeeded);
        }

        [Fact]
        [Trait("Category", "SignUp")]
        public async Task Succeed_OnSignUp()
        {
            // Arrange

            RegisterIdentityUserRequest? message = null;

            var request = new SignUpRequest
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                PhoneNumber = "+380000000000",
                Password = "password",
            };

            var user = new User();

            _userManager
                .Setup(e => e.CreateAsync(user, request.Password))
                .ReturnsAsync(IdentityResult.Success);

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<RegisterIdentityUserRequest>()))
                .Callback<RegisterIdentityUserRequest>(e => message = e);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<User>(request))
                .Returns(user);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(false),
            };

            // Act

            var result = await sut.SignUp(request, _userManager.Object, producer.Object, mapper.Object);

            // Assert

            _userManager.VerifyAll();
            producer.VerifyAll();
            mapper.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignUpResponse>((result.Result as OkObjectResult).Value);
            Assert.True(((result.Result as OkObjectResult).Value as SignUpResponse).Succeeded);

            Assert.NotNull(message);
            Assert.Equal(user.Id, message.IdentityUserId);
            Assert.Equal(request.FirstName, message.FirstName);
            Assert.Equal(request.LastName, message.LastName);
            Assert.Equal(request.PhoneNumber, message.PhoneNumber);
            Assert.Equal(request.Email, message.Email);
        }

        [Fact]
        [Trait("Category", "SignIn")]
        public async Task Fail_WhenIsAuthenticated_OnSignIn()
        {
            // Arrange

            var request = new SignInRequest();

            var interactionService = new Mock<IIdentityServerInteractionService>();

            var serverUrls = new Mock<IServerUrls>();

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(true),
            };

            // Act

            var result = await sut.SignIn(request, interactionService.Object, _signInManager.Object, serverUrls.Object);

            // Assert

            interactionService.VerifyAll();
            _signInManager.VerifyAll();
            serverUrls.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignInResponse>((result.Result as OkObjectResult).Value);
            Assert.False(((result.Result as OkObjectResult).Value as SignInResponse).Succeeded);
        }

        [Fact]
        [Trait("Category", "SignIn")]
        public async Task Fail_WhenIsNotAuthenticated_OnSignIn()
        {
            // Arrange

            var request = new SignInRequest();

            var interactionService = new Mock<IIdentityServerInteractionService>();

            _signInManager
                .Setup(e => e.PasswordSignInAsync(request.Username, request.Password, request.Remember, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var serverUrls = new Mock<IServerUrls>();

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(false),
            };

            // Act

            var result = await sut.SignIn(request, interactionService.Object, _signInManager.Object, serverUrls.Object);

            // Assert

            interactionService.VerifyAll();
            _signInManager.VerifyAll();
            serverUrls.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignInResponse>((result.Result as OkObjectResult).Value);
            Assert.False(((result.Result as OkObjectResult).Value as SignInResponse).Succeeded);
        }

        [Fact]
        [Trait("Category", "SignIn")]
        public async Task Succeed_WithoutAuthorizationContext_OnSignIn()
        {
            // Arrange

            var request = new SignInRequest
            {
                Username = "john.smith@example.com",
                Password = "password",
                Remember = true,
            };

            var interactionService = new Mock<IIdentityServerInteractionService>();
            interactionService
                .Setup(e => e.GetAuthorizationContextAsync(request.ReturnUrl))
                .ReturnsAsync(default(AuthorizationRequest));

            _signInManager
                .Setup(e => e.PasswordSignInAsync(request.Username, request.Password, request.Remember, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var baseUrl = Guid.NewGuid().ToString();

            var serverUrls = new Mock<IServerUrls>();
            serverUrls
                .Setup(e => e.BaseUrl)
                .Returns(baseUrl);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(false),
            };

            // Act

            var result = await sut.SignIn(request, interactionService.Object, _signInManager.Object, serverUrls.Object);

            // Assert

            interactionService.VerifyAll();
            _signInManager.VerifyAll();
            serverUrls.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignInResponse>((result.Result as OkObjectResult).Value);
            Assert.True(((result.Result as OkObjectResult).Value as SignInResponse).Succeeded);
            Assert.Equal(baseUrl, ((result.Result as OkObjectResult).Value as SignInResponse).ValidReturnUrl);
        }

        [Fact]
        [Trait("Category", "SignIn")]
        public async Task Succeed_WithAuthorizationContext_OnSignIn()
        {
            // Arrange

            var request = new SignInRequest
            {
                Username = "john.smith@example.com",
                Password = "password",
                Remember = true,
                ReturnUrl = Guid.NewGuid().ToString(),
            };

            var authorizationRequest = new AuthorizationRequest();

            var interactionService = new Mock<IIdentityServerInteractionService>();
            interactionService
                .Setup(e => e.GetAuthorizationContextAsync(request.ReturnUrl))
                .ReturnsAsync(authorizationRequest);

            _signInManager
                .Setup(e => e.PasswordSignInAsync(request.Username, request.Password, request.Remember, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var serverUrls = new Mock<IServerUrls>();

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(false),
            };

            // Act

            var result = await sut.SignIn(request, interactionService.Object, _signInManager.Object, serverUrls.Object);

            // Assert

            interactionService.VerifyAll();
            _signInManager.VerifyAll();
            serverUrls.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignInResponse>((result.Result as OkObjectResult).Value);
            Assert.True(((result.Result as OkObjectResult).Value as SignInResponse).Succeeded);
            Assert.Equal(request.ReturnUrl, ((result.Result as OkObjectResult).Value as SignInResponse).ValidReturnUrl);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "SignOut")]
        public async Task Succeed_WhenNotPrompt_OnSignOut(bool isAuthenticated)
        {
            // Arrange

            var logoutId = Guid.NewGuid().ToString();
            var iframeUrl = Guid.NewGuid().ToString();
            var logoutInfo = new LogoutRequest(iframeUrl, new LogoutMessage
            {
                ClientId = Guid.NewGuid().ToString(),
            });

            var interactionService = new Mock<IIdentityServerInteractionService>();
            interactionService
                .Setup(e => e.GetLogoutContextAsync(logoutId))
                .ReturnsAsync(logoutInfo);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(isAuthenticated),
            };

            // Act

            var result = await sut.SignOut(logoutId, interactionService.Object, _signInManager.Object);

            // Assert

            interactionService.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignOutInfo>((result.Result as OkObjectResult).Value);
            Assert.False(((result.Result as OkObjectResult).Value as SignOutInfo).Prompt);
            Assert.NotNull(((result.Result as OkObjectResult).Value as SignOutInfo).PostInfo);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "SignOut")]
        public async Task Succeed_WhenPrompt_OnSignOut(bool isAuthenticated)
        {
            // Arrange

            var logoutId = Guid.NewGuid().ToString();
            var iframeUrl = Guid.NewGuid().ToString();
            var logoutInfo = new LogoutRequest(iframeUrl, null);

            var interactionService = new Mock<IIdentityServerInteractionService>();
            interactionService
                .Setup(e => e.GetLogoutContextAsync(logoutId))
                .ReturnsAsync(logoutInfo);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(isAuthenticated),
            };

            // Act

            var result = await sut.SignOut(logoutId, interactionService.Object, _signInManager.Object);

            // Assert

            interactionService.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignOutInfo>((result.Result as OkObjectResult).Value);
            Assert.True(((result.Result as OkObjectResult).Value as SignOutInfo).Prompt);
            Assert.Null(((result.Result as OkObjectResult).Value as SignOutInfo).PostInfo);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [Trait("Category", "PostSignOut")]
        public async Task SignOut(bool isAuthenticated)
        {
            // Arrange

            var logoutId = Guid.NewGuid().ToString();
            var iframeUrl = Guid.NewGuid().ToString();
            var logoutInfo = new LogoutRequest(iframeUrl, new LogoutMessage
            {
                ClientId = Guid.NewGuid().ToString(),
            });

            var interactionService = new Mock<IIdentityServerInteractionService>();
            interactionService
                .Setup(e => e.GetLogoutContextAsync(logoutId))
                .ReturnsAsync(logoutInfo);

            var sut = new AuthController
            {
                ControllerContext = GenerateControllerContext(isAuthenticated),
            };

            // Act

            var result = await sut.PostSignOut(logoutId, interactionService.Object, _signInManager.Object);

            // Assert

            interactionService.VerifyAll();

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<SignOutInfo>((result.Result as OkObjectResult).Value);
            Assert.Null(((result.Result as OkObjectResult).Value as SignOutInfo).Prompt);
            Assert.NotNull(((result.Result as OkObjectResult).Value as SignOutInfo).PostInfo);
        }

        private ControllerContext GenerateControllerContext(bool isAuthenticated)
        {
            var authenticationType = isAuthenticated ? "mock" : null;
            var user = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(authenticationType),
            });
            var controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user,
                },
            };
            return controllerContext;
        }
    }
}
