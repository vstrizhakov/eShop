using eShopping.Bots.Common;

namespace eShopping.ViberBot.Framework
{
    public interface IViberView
    {
        Message Build(IBotContextConverter botContextConverter);
    }
}