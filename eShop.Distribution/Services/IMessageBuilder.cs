using eShop.Distribution.Entities.History;
using eShop.Messaging.Contracts;

namespace eShop.Distribution.Services
{
    public interface IMessageBuilder
    {
        Message FromComposition(Announce composition, DistributionSettingsRecord distributionSettings, ITextFormatter formatter);
    }
}
