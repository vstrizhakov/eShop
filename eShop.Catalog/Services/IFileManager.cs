namespace eShop.Catalog.Services
{
    public interface IFileManager
    {
        Task<string> SaveAsync(string directory, string extension, Stream stream);
        Task DeleteAsync(string path);
    }
}
