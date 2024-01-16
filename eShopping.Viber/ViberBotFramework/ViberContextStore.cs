using eShopping.ViberBot;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Services;

namespace eShopping.Viber.ViberBotFramework
{
    public class ViberContextStore : IContextStore
    {
        private readonly IViberService _viberService;

        public ViberContextStore(IViberService viberService)
        {
            _viberService = viberService;
        }

        public async Task<string?> GetActiveContextAsync(Callback callback)
        {
            var activeContext = default(string);
            if (callback.Event == EventType.Message)
            {
                var sender = callback.Sender;
                if (sender != null)
                {
                    var user = await _viberService.GetUserByIdAsync(sender.Id);
                    if (user != null)
                    {
                        activeContext = user.ActiveContext;
                    }
                }
            }

            return activeContext;
        }

        public async Task SetActiveContextAsync(Callback callback, string? activeContext)
        {
            if (callback.Event == EventType.Message)
            {
                var sender = callback.Sender;
                if (sender != null)
                {
                    var user = await _viberService.GetUserByIdAsync(sender.Id);
                    if (user != null)
                    {
                        await _viberService.SetActiveContextAsync(user, activeContext);
                    }
                }
            }
        }
    }
}
