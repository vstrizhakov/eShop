namespace eShop.Bots.Common
{
    public interface IBotContextConverter
    {
        string Serialize(string action, params string[] args);
        string[] Deserialize(string context);
    }
}
