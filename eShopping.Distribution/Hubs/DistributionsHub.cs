using AutoMapper;
using eShopping.Common.Extensions;
using eShopping.Distribution.Models.Distributions;
using eShopping.Distribution.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace eShopping.Distribution.Hubs
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
            var distribution = await _distributionService.GetDistributionAsync(distributionId, userId.Value);
            if (distribution == null || distribution.AnnouncerId != userId)
            {
                throw new InvalidOperationException("The specified distribution doesn't exist.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, distributionId.ToString());

            foreach (var target in distribution.Targets)
            {
                var items = _mapper.Map<IEnumerable<DistributionItem>>(target.Items);
                foreach (var item in items)
                {
                    await Clients.Caller.RequestUpdated(item);
                }
            }
        }

        public async Task Unsubscribe(UnsubscribeFromDistributionRequest request)
        {
            var distributionId = request.DistributionId;

            var userId = Context.User.GetAccountId();
            var distribution = await _distributionService.GetDistributionAsync(distributionId, userId.Value);
            if (distribution == null || distribution.AnnouncerId != userId)
            {
                throw new InvalidOperationException("The specified distribution doesn't exist.");
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, distributionId.ToString());
        }
    }
}
