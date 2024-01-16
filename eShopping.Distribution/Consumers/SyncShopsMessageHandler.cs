using AutoMapper;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts.Catalog;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SyncShopsMessageHandler : IConsumer<SyncShopsMessage>
    {
        private readonly IMapper _mapper;
        private readonly IShopService _shopService;

        public SyncShopsMessageHandler(IMapper mapper, IShopService shopService)
        {
            _mapper = mapper;
            _shopService = shopService;
        }

        public async Task Consume(ConsumeContext<SyncShopsMessage> context)
        {
            var command = context.Message;
            var shops = _mapper.Map<IEnumerable<Entities.Shop>>(command.Shops);
            await _shopService.SyncShopsAsync(shops);
        }
    }
}
