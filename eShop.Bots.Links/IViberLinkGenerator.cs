namespace eShop.Bots.Links
{
    public interface IViberLinkGenerator
    {
        string Generate();
        string Generate(string action, params string[] args);
    }
}
