namespace eShop.Bots.Links
{
    public interface ITelegramLinkGenerator
    {
        string Generate();
        string Generate(string action, params string[] args);
    }
}
