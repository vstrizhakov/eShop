using AutoMapper;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Distribution.Controllers
{
    [Route("api/distribution")]
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
            if (distributionGroup == null)
            {
                return NotFound();
            }

            var distribution = mapper.Map<Models.Distribution>(distributionGroup);
            return Ok(distribution);
        }
    }
}
