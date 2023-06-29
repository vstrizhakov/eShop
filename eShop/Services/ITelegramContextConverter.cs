namespace eShop.Services
{
    public interface ITelegramContextConverter
    {
        string Serialize(params string[] args);
        string[] Deserialize(string context);
    }
}
