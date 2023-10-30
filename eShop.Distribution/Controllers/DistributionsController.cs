using AutoMapper;
using eShop.Common.Extensions;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distributions")]
    [ApiController]
    [Authorize]
    public class DistributionsController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Distribution>> GetDistribution(
            [FromRoute] Guid id,
            [FromServices] IDistributionRepository distributionRepository,
            [FromServices] IMapper mapper)
        {
            var distributionGroup = await distributionRepository.GetDistributionGroupByIdAsync(id);

            var providerId = User.GetAccountId().Value;
            if (distributionGroup == null || distributionGroup.ProviderId != providerId)
            {
                return NotFound();
            }

            var distribution = mapper.Map<Models.Distribution>(distributionGroup);
            return Ok(distribution);
        }
    }
}
