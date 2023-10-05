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
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public DistributionSettingsController(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<DistributionSettings>> GetDistributionSettings()
        {
            var accountId = User.GetAccountId().Value;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            var response = _mapper.Map<DistributionSettings>(distributionSettings);
            return Ok(response);
        }
    }
}
