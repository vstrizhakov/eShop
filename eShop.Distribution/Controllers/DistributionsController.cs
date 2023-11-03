using AutoMapper;
using eShop.Common.Extensions;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distribution/distributions")]
    [ApiController]
    [Authorize]
    public class DistributionsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Distributions.Distribution>> GetDistribution(
            [FromRoute] Guid id,
            [FromServices] IDistributionRepository distributionRepository,
            [FromServices] IMapper mapper)
        {
            var distribution = await distributionRepository.GetDistributionByIdAsync(id);

            var providerId = User.GetAccountId().Value;
            if (distribution == null || distribution.ProviderId != providerId)
            {
                return NotFound();
            }

            var mappedDistribution = mapper.Map<Models.Distributions.Distribution>(distribution);
            return Ok(mappedDistribution);
        }
    }
}
