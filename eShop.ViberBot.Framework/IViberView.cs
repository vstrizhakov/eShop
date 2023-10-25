using eShop.Bots.Common;

namespace eShop.ViberBot.Framework
{
    public interface IViberView
    {
        Message Build(IBotContextConverter botContextConverter);
    }
}