using AutoMapper;
using eShop.Common.Extensions;
using eShop.Distribution.Models;
using eShop.Distribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distribution/clients/{clientId}/settings")]
    [ApiController]
    [Authorize]
    public class DistributionSettingsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public DistributionSettingsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<DistributionSettings>> GetDistributionSettings()
        {
            var accountId = User.GetAccountId().Value;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            var response = _mapper.Map<DistributionSettings>(account.DistributionSettings);
            return Ok(response);
        }
    }
}
