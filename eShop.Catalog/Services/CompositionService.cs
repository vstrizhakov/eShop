using eShop.Catalog.Entities;
using eShop.Catalog.Repositories;
using eShop.Common;
using eShop.Messaging;

namespace eShop.Catalog.Services
{
    public class CompositionService : ICompositionService
    {
        private readonly IFileManager _fileManager;
        private readonly ICompositionRepository _compositionRepository;
        private readonly IPublicUriBuilder _publicUriBuilder;
        private readonly IProducer _producer;

        public CompositionService(
            IFileManager fileManager,
            ICompositionRepository compositionRepository,
            IPublicUriBuilder publicUriBuilder,
            IProducer producer)
        {
            _fileManager = fileManager;
            _compositionRepository = compositionRepository;
            _publicUriBuilder = publicUriBuilder;
            _producer = producer;
        }

        public async Task<IEnumerable<Composition>> GetCompositionsAsync(Guid ownerId)
        {
            var compositions = await _compositionRepository.GetCompositionsAsync(ownerId);
            return compositions;
        }

        public async Task<Composition?> GetCompositionAsync(Guid id)
        {
            var composition = await _compositionRepository.GetCompositionByIdAsync(id);
            return composition;
        }

        public async Task CreateCompositionAsync(Composition composition, IFormFile image)
        {
            if (composition == null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            using var imageStream = image.OpenReadStream();
            var imagePath = await _fileManager.SaveAsync(Path.Combine("Catalog", "Compositions", composition.Id.ToString(), "Images"), Path.GetExtension(image.FileName), imageStream);

            // TODO: Handle products` images (request.Products.Images)

            composition.Images.Add(new CompositionImage
            {
                Path = imagePath,
            });

            await _compositionRepository.CreateCompositionAsync(composition);

            var broadcastMessage = new Messaging.Models.BroadcastCompositionMessage
            {
                ProviderId = composition.OwnerId,
                Composition = new Messaging.Models.Composition
                {
                    Id = composition.Id,
                    Images = composition.Images.Select(e => new Uri(_publicUriBuilder.Path(e.Path))).ToList(),
                    Products = composition.Products.Select(e => new Messaging.Models.Product
                    {
                        Name = e.Name,
                        Url = e.Url,
                        Price = e.Prices.FirstOrDefault().Value,
                    }).ToList(),
                },
            };
            _producer.Publish(broadcastMessage);
        }

        public async Task DeleteCompositionAsync(Composition composition)
        {
            if (composition == null)
            {
                throw new ArgumentNullException(nameof(composition));
            }

            foreach (var image in composition.Images)
            {
                await _fileManager.DeleteAsync(image.Path);
            }

            await _compositionRepository.DeleteCompositionAsync(composition);
        }
    }
}
