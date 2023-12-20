using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Repositories;
using eShop.Common;
using MassTransit;

namespace eShop.Catalog.Services
{
    public class AnnounceService : IAnnouncesService
    {
        private readonly IFileManager _fileManager;
        private readonly IAnnounceRepository _announceRepository;
        private readonly IPublicUriBuilder _publicUriBuilder;
        private readonly IBus _producer;
        private readonly IAnnouncesHubServer _announcesHubServer;
        private readonly IMapper _mapper;

        public AnnounceService(
            IFileManager fileManager,
            IAnnounceRepository announceRepository,
            IPublicUriBuilder publicUriBuilder,
            IBus producer,
            IAnnouncesHubServer announcesHubServer,
            IMapper mapper)
        {
            _fileManager = fileManager;
            _announceRepository = announceRepository;
            _publicUriBuilder = publicUriBuilder;
            _producer = producer;
            _announcesHubServer = announcesHubServer;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Announce>> GetAnnouncesAsync(Guid ownerId)
        {
            var announces = await _announceRepository.GetAnnouncesAsync(ownerId);
            return announces;
        }

        public async Task<Announce?> GetAnnounceAsync(Guid id, Guid ownerId)
        {
            var announce = await _announceRepository.GetAnnounceAsync(id, ownerId);
            return announce;
        }

        public async Task CreateAnnounceAsync(Announce announce, IFormFile image)
        {
            if (announce == null)
            {
                throw new ArgumentNullException(nameof(announce));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            announce.Id = Guid.NewGuid();

            using var imageStream = image.OpenReadStream();
            var imagePath = await _fileManager.SaveAsync(Path.Combine("Announces", announce.Id.ToString(), "Images"), Path.GetExtension(image.FileName), imageStream);

            // TODO: Handle products` images (request.Products.Images)

            announce.Images.Add(new AnnounceImage
            {
                Path = imagePath,
            });

            await _announceRepository.CreateAnnounceAsync(announce);

            var broadcastMessage = new Messaging.Contracts.BroadcastAnnounceMessage
            {
                AnnouncerId = announce.OwnerId,
                Announce = new Messaging.Contracts.Announce
                {
                    Id = announce.Id,
                    ShopId = announce.Shop.Id,
                    Images = announce.Images.Select(e => new Uri(_publicUriBuilder.Path(e.Path))).ToList(),
                    Products = announce.Products.Select(e =>
                    {
                        var price = e.Prices.FirstOrDefault();
                        var currency = price.Currency;
                        var product = new Messaging.Contracts.Product
                        {
                            Name = e.Name,
                            Description = e.Description,
                            Url = e.Url,
                            Price = new Messaging.Contracts.ProductPrice
                            {
                                Price = price.Value,
                                DiscountedPrice = price.DiscountedValue,
                                Currency = _mapper.Map<Messaging.Contracts.Currency>(currency),
                            },
                        };

                        return product;
                    }).ToList(),
                },
            };

            await _producer.Publish(broadcastMessage);
        }

        public async Task DeleteAnnounceAsync(Announce announce)
        {
            if (announce == null)
            {
                throw new ArgumentNullException(nameof(announce));
            }

            foreach (var image in announce.Images)
            {
                await _fileManager.DeleteAsync(image.Path);
            }

            await _announceRepository.DeleteAnnounceAsync(announce);
        }

        public async Task SetAnnounceDistributionIdAsync(Announce announce, Guid distributionId)
        {
            if (announce == null)
            {
                throw new ArgumentNullException(nameof(announce));
            }

            announce.DistributionId = distributionId;

            await _announceRepository.UpdateAnnounceAsync(announce);

            await _announcesHubServer.SendAnnounceUpdatedAsync(announce);
        }
    }
}
