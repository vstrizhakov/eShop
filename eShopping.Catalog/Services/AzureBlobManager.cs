using Azure.Identity;
using Azure.Storage.Blobs;

namespace eShopping.Catalog.Services
{
    public class AzureBlobManager : IFileManager
    {
        private readonly BlobContainerClient _client;

        public AzureBlobManager()
        {
            _client = new BlobContainerClient(
                new Uri("https://eshopstorage.blob.core.windows.net/catalog"),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions
                {
                    TenantId = "1da49a78-93f5-4f9d-b9ab-da82fc5db577",
                }));
        }
        public async Task DeleteAsync(string path)
        {
            await _client.DeleteBlobAsync(path);
        }

        public async Task<string> SaveAsync(string directory, string extension, Stream stream)
        {
            var filename = Path.ChangeExtension(Guid.NewGuid().ToString(), extension);
            var path = Path.Combine(directory, filename);

            await _client.UploadBlobAsync(path, stream);

            return $"https://eshopcdn.azureedge.net/catalog/{path}";
        }
    }
}
