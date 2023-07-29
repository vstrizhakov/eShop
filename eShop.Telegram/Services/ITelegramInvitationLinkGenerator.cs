namespace eShop.Telegram.Services
{
    public interface ITelegramInvitationLinkGenerator
    {
        string Generate(Guid providerId);
    }
}
