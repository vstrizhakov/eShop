namespace eShop.Services
{
    public class FileManager : IFileManager
    {
        private readonly string _rootDirectory;

        public FileManager(IWebHostEnvironment environment)
        {
            _rootDirectory = environment.WebRootPath;
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
