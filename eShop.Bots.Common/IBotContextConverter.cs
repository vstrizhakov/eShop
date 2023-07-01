namespace eShop.Bots.Common
{
    public interface IBotContextConverter
    {
        string Serialize(params string[] args);
        string[] Deserialize(string context);
    }
}
