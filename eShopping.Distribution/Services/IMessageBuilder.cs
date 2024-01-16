using eShopping.Distribution.Entities.History;
using eShopping.Messaging.Contracts;

namespace eShopping.Distribution.Services
{
    public interface IMessageBuilder
    {
        Message FromComposition(Announce composition, DistributionSettingsRecord distributionSettings, ITextFormatter formatter);
    }
}
