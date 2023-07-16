using eShop.Catalog.Services;
using Microsoft.Extensions.Options;
using Moq;

namespace eShop.Catalog.Tests.Services
{
    public class FileManagerShould : IClassFixture<FileManagerFixture>
    {
        private readonly FileManagerFixture _fixture;
        private readonly Mock<IOptions<FilesConfiguration>> _fileConfigurationOptions;

        public FileManagerShould(FileManagerFixture fixture)
        {
            _fixture = fixture;

            _fileConfigurationOptions = new Mock<IOptions<FilesConfiguration>>();
            _fileConfigurationOptions
                .Setup(e => e.Value)
                .Returns(new FilesConfiguration
                {
                    Root = "Files",
                });
        }

        [Fact]
        public async Task SaveFile()
        {
            // Arrange

            var options = _fileConfigurationOptions.Object;

            // Act

            var fileManager = new FileManager(options);

            using var memoryStream = new MemoryStream(new byte[8 * 1024 * 1024]);
            var path = await fileManager.SaveAsync("Directory", ".txt", memoryStream);

            // Assert

            Assert.True(File.Exists(Path.Combine(options.Value.Root, path)));
            
            _fixture.Path = path;
        }

        [Fact]
        public async Task DeleteFile()
        {
            // Arrange
                
            var options = _fileConfigurationOptions.Object;

            // Act

            var fileManager = new FileManager(options);

            await fileManager.DeleteAsync(_fixture.Path);

            // Assert

            Assert.False(File.Exists(Path.Combine(options.Value.Root, _fixture.Path)));
        }
    }
}
