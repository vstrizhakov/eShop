namespace eShop.Services
{
    public interface IFileManager
    {
        Task<string> SaveAsync(string directory, string extendsion, Stream stream);
        Task DeleteAsync(string path);
    }
}
