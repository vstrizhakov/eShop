﻿using eShopping.Catalog.Entities;
using eShopping.Catalog.Repositories;
using eShopping.Catalog.Services;
using eShopping.Common;
using eShopping.Messaging;
using eShopping.Messaging.Contracts;
using Microsoft.AspNetCore.Http;

namespace eShopping.Catalog.Tests.Services
{
    public class CompositionServiceShould
    {
        [Fact]
        public async Task ReturnCompositions_OnGetCompositions()
        {
            // Arrange

            var ownerId = Guid.NewGuid();

            var compositions = Array.Empty<Announce>();

            var fileManager = new Mock<IFileManager>();

            var compositionRepository = new Mock<IAnnounceRepository>();
            compositionRepository
                .Setup(e => e.GetAnnouncesAsync(ownerId))
                .ReturnsAsync(compositions);

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act

            var result = await service.GetCompositionsAsync(ownerId);

            // Assert

            fileManager.VerifyAll();
            compositionRepository.VerifyAll();
            publicUriBuilder.VerifyAll();
            producer.VerifyAll();

            Assert.Equal(compositions, result);
        }

        [Fact]
        public async Task ReturnComposition_OnGetComposition()
        {
            // Arrange

            var composition = new Announce();
            var compositionId = composition.Id;

            var fileManager = new Mock<IFileManager>();

            var compositionRepository = new Mock<IAnnounceRepository>();
            compositionRepository
                .Setup(e => e.GetAnnounceByIdAsync(compositionId))
                .ReturnsAsync(composition);

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act

            var result = await service.GetCompositionAsync(compositionId);

            // Assert

            fileManager.VerifyAll();
            compositionRepository.VerifyAll();
            publicUriBuilder.VerifyAll();
            producer.VerifyAll();

            Assert.Equal(composition, result);
        }

        [Fact]
        public async Task CreateComposition()
        {
            // Arrange
            BroadcastAnnounceMessage? result = null;

            var composition = new Announce
            {
                OwnerId = Guid.NewGuid(),
            };
            var stream = new MemoryStream();
            var fileName = "example.png";

            var image = new Mock<IFormFile>();
            image
                .Setup(e => e.OpenReadStream())
                .Returns(stream);
            image
                .Setup(e => e.FileName)
                .Returns(fileName);

            var directory = Path.Combine("Catalog", "Compositions", composition.Id.ToString(), "Images");
            var extension = Path.GetExtension(fileName);

            var imagePath = Path.Combine(directory, extension);

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(e => e.SaveAsync(directory, extension, stream))
                .ReturnsAsync(imagePath);

            var compositionRepository = new Mock<IAnnounceRepository>();
            compositionRepository
                .Setup(e => e.CreateAnnounceAsync(composition))
                .Returns(Task.CompletedTask);

            var publicUriBuilder = new Mock<IPublicUriBuilder>();
            publicUriBuilder
                .Setup(e => e.Path(imagePath))
                .Returns((string relativePath) => new Uri(new Uri("https://example.com"), relativePath).ToString());

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<BroadcastAnnounceMessage>()))
                .Callback<BroadcastAnnounceMessage>(message => result = message);

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act

            await service.CreateCompositionAsync(composition, image.Object);

            // Assert

            image.VerifyAll();
            fileManager.VerifyAll();
            compositionRepository.VerifyAll();
            publicUriBuilder.VerifyAll();
            producer.VerifyAll();

            Assert.NotEmpty(composition.Images);
            Assert.NotNull(result);
            Assert.Equal(composition.OwnerId, result.AnnouncerId);
            Assert.NotNull(result.Announce);
        }

        [Fact]
        public async Task ThrowArgumentNullException_WhenCompositionNull_OnCreateComposition()
        {
            // Arrange

            var image = new Mock<IFormFile>();

            var fileManager = new Mock<IFileManager>();

            var compositionRepository = new Mock<IAnnounceRepository>();

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act & Assert

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateCompositionAsync(null, image.Object);
            });
        }

        [Fact]
        public async Task ThrowArgumentNullException_WhenImageNull_OnCreateComposition()
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var compositionRepository = new Mock<IAnnounceRepository>();

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act & Assert

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateCompositionAsync(new Announce(), null);
            });
        }

        [Fact]
        public async Task DeleteComposition()
        {
            // Arrange

            var composition = new Announce
            {
                Images =
                {
                    new AnnounceImage
                    {
                    },
                    new AnnounceImage
                    {
                    },
                },
            };

            var fileManager = new Mock<IFileManager>();
            fileManager
                .Setup(e => e.DeleteAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            var compositionRepository = new Mock<IAnnounceRepository>();
            compositionRepository
                .Setup(e => e.DeleteAnnounceAsync(composition))
                .Returns(Task.CompletedTask);

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act

            await service.DeleteCompositionAsync(composition);

            // Assert

            fileManager.VerifyAll();
            compositionRepository.VerifyAll();
            publicUriBuilder.VerifyAll();
            producer.VerifyAll();

            fileManager.Verify(e => e.DeleteAsync(It.IsAny<string>()), Times.Exactly(composition.Images.Count()));
        }

        [Fact]
        public async Task ThrowArgumentNullException_WhenCompositionNull_OnDeleteComposition()
        {
            // Arrange

            var fileManager = new Mock<IFileManager>();

            var compositionRepository = new Mock<IAnnounceRepository>();

            var publicUriBuilder = new Mock<IPublicUriBuilder>();

            var producer = new Mock<IProducer>();

            var service = new CompositionService(fileManager.Object, compositionRepository.Object, publicUriBuilder.Object, producer.Object);

            // Act & Assert

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.DeleteCompositionAsync(null);
            });
        }
    }
}
