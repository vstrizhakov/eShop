namespace eShopping.ViberBot
{
    public class ViberBotClientOptions
    {
        public Uri Host { get; } = new Uri("https://chatapi.viber.com");
        public string Token { get; }

        public ViberBotClientOptions(string token)
        {
            Token = token;
        }
    }
}
