using AutoMapper;
using eShopping.Catalog.Entities;
using eShopping.Catalog.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace eShopping.Catalog.Services
{
    public class AnnounceHubServer : IAnnouncesHubServer
    {
        private readonly IHubContext<AnnouncesHub, IAnnouncesHubClient> _hubContext;
        private readonly IMapper _mapper;

        public AnnounceHubServer(IHubContext<AnnouncesHub, IAnnouncesHubClient> hubContext, IMapper mapper)
        {
            _hubContext = hubContext;
            _mapper = mapper;
        }

        public async Task SendAnnounceUpdatedAsync(Announce announce)
        {
            var announceId = announce.Id;
            var mappedAnnounce = _mapper.Map<Models.Announces.Announce>(announce);
            await _hubContext.Clients.Group(announceId.ToString()).AnnounceUpdated(mappedAnnounce);
        }
    }
}
