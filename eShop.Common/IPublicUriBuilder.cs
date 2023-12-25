namespace eShop.Common
{
    public interface IPublicUriBuilder
    {
        string FrontendPath(string relativePath);
        string BackendPath(string relativePath);
    }
}
