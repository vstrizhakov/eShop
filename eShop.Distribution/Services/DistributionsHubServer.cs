using AutoMapper;
using eShop.Distribution.Entities;
using eShop.Distribution.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace eShop.Distribution.Services
{
    public class DistributionsHubServer : IDistributionsHubServer
    {
        private readonly IHubContext<DistributionsHub, IDistributionsHubClient> _hubContext;
        private readonly IMapper _mapper;

        public DistributionsHubServer(IHubContext<DistributionsHub, IDistributionsHubClient> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task SendRequestUpdatedAsync(Entities.DistributionItem request)
        {
            var distributionId = request.DistributionId;

            var mappedRequest = _mapper.Map<Models.Distributions.DistributionItem>(request);
            await _hubContext.Clients.Group(distributionId.ToString()).RequestUpdated(mappedRequest);
        }
    }
}
