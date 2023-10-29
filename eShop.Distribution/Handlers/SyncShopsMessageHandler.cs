using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Catalog;

namespace eShop.Distribution.Handlers
{
    public class SyncShopsMessageHandler : IMessageHandler<SyncShopsMessage>
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;

        public SyncShopsMessageHandler(IMapper mapper, IShopService shopService)
        {
            _mapper = mapper;
            _shopService = shopService;
        }

        public async Task HandleMessageAsync(SyncShopsMessage message)
        {
            var shops = _mapper.Map<IEnumerable<Entities.Shop>>(message.Shops);
            await _shopService.SyncShopsAsync(shops);
        }
    }
}
