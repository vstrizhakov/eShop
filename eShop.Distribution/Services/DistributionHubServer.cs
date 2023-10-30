using AutoMapper;
using eShop.Distribution.Entities;
using eShop.Distribution.Hubs;
using eShop.Distribution.Models;
using Microsoft.AspNetCore.SignalR;

namespace eShop.Distribution.Services
{
    public class DistributionHubServer : IDistributionHubServer
    {
        private readonly IHubContext<DistributionHub, IDistributionClient> _hubContext;
        private readonly IMapper _mapper;

        public DistributionHubServer(IHubContext<DistributionHub, IDistributionClient> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task SendRequestUpdatedAsync(DistributionGroupItem request)
        {
            var distributionId = request.GroupId;

            var mappedRequest = _mapper.Map<DistributionItem>(request);
            await _hubContext.Clients.Group(distributionId.ToString()).RequestUpdated(mappedRequest);
        }
    }
}
