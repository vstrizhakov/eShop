using Microsoft.Extensions.Options;

namespace eShop.Catalog.Services
{
    public class FileManager : IFileManager
    {
        private readonly string _rootDirectory;

        public FileManager(IOptions<FilesConfiguration> options)
        {
            _rootDirectory = options.Value.Root;
        }

        public async Task<string> SaveAsync(string directory, string extendsion, Stream stream)
        {
            var directoryPath = Path.Combine(_rootDirectory, directory);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var filename = Path.ChangeExtension(Guid.NewGuid().ToString(), extendsion);
            var path = Path.Combine(directoryPath, filename);

            using var fileStream = File.Create(path);
            await stream.CopyToAsync(fileStream);

            return Path.GetRelativePath(_rootDirectory, path);
        }

        public Task DeleteAsync(string path)
        {
            var fullPath = Path.Combine(_rootDirectory, path);
            File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
