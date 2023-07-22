using eShop.Messaging.Models;

namespace eShop.Distribution.Services
{
    public class MessageBuilder : IMessageBuilder
    {
        public Message FromComposition(Composition composition)
        {
            var image = composition.Images.FirstOrDefault();
            var caption = string.Join("\n\n", composition.Products.Select(e => $"{e.Name} - {e.Price}"));

            var message = new Message
            {
                Image = image,
                Caption = caption,
            };
            return message;
        }
    }
}
