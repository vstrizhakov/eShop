namespace eShop.Viber.Services
{
    public interface IViberInvitationLinkGenerator
    {
        string Generate(Guid providerId);
    }
}
