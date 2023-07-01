using System.Text;

namespace eShop.Bots.Common
{
    internal class BotContextConverter : IBotContextConverter
    {
        public string[] Deserialize(string context)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(context)).Split(',');
        }

        public string Serialize(params string[] args)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(',', args)));
        }
    }
}
