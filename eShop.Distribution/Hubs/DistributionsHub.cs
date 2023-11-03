using AutoMapper;
using eShop.Common.Extensions;
using eShop.Distribution.Models.Distributions;
using eShop.Distribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace eShop.Distribution.Hubs
{
    [Authorize]
    public class DistributionsHub : Hub<IDistributionsHubClient>
    {
        private readonly IDistributionService _distributionService;
        private readonly IMapper _mapper;

        public DistributionsHub(IDistributionService distributionService, IMapper mapper)
        {
            _distributionService = distributionService;
            _mapper = mapper;
        }

        public async Task Subscribe(SubscribeToDistributionRequest request)
        {
            var distributionId = request.DistributionId;

            var userId = Context.User.GetAccountId();
            var distribution = await _distributionService.GetDistributionAsync(distributionId);
            if (distribution == null || distribution.ProviderId != userId)
            {
                throw new InvalidOperationException("The specified distribution doesn't exist.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, distributionId.ToString());

            var items = _mapper.Map<IEnumerable<DistributionItem>>(distribution.Items);
            foreach (var item in items)
            {
                await Clients.Caller.RequestUpdated(item);
            }
        }

        public async Task Unsubscribe(UnsubscribeFromDistributionRequest request)
        {
            var distributionId = request.DistributionId;

            var userId = Context.User.GetAccountId();
            var distribution = await _distributionService.GetDistributionAsync(distributionId);
            if (distribution == null || distribution.ProviderId != userId)
            {
                throw new InvalidOperationException("The specified distribution doesn't exist.");
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, distributionId.ToString());
        }
    }
}
