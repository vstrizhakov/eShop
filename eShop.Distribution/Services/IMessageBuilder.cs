using eShop.Messaging.Models;

namespace eShop.Distribution.Services
{
    public interface IMessageBuilder
    {
        Message FromComposition(Composition composition);
    }
}
