using Telegram.Bot.Types;

namespace eShop.Telegram.Services
{
    public class RateLimiter : IRateLimiter
    {
        private const uint MaxRateForChatPerSecond = 1;
        private const uint MaxRateForMultipleChatsPerSecond = 30;
        private const uint MaxRateForGroupPerMinute = 20;

        struct State
        {
            public DateTime LastTick { get; set; } = DateTime.UtcNow;
            public uint Counter { get; set; } = 0;

            public State()
            {
            }
        }

        private readonly object _lock = new();

        // Particular chat limits
        private IDictionary<ChatId, State> _chatCounters = new Dictionary<ChatId, State>();

        // Multiple users limits
        private DateTime? _lastTick;
        private IList<ChatId> _chats = new List<ChatId>();

        // Group chat limits
        private IDictionary<ChatId, State> _groupCounters = new Dictionary<ChatId, State>();

        public async Task AcquireTimeslotAsync(ChatId chatId, bool isGroup)
        {
            bool result;
            do
            {
                result = TryAcquireTimeslot(chatId, isGroup, out var waitTime);
                if (!result)
                {
                    await Task.Delay(waitTime);
                }
            } while (!result);
        }

        private bool TryAcquireTimeslot(ChatId chatId, bool isGroup, out TimeSpan waitTime)
        {
            var result = true;
            var resultWaitTime = TimeSpan.Zero;

            void SetWaitTime(TimeSpan value)
            {
                if (value > resultWaitTime)
                {
                    resultWaitTime = value;
                }
            }

            lock (_lock)
            {
                var now = DateTime.UtcNow;

                State state;
                {
                    var range = TimeSpan.FromSeconds(1);

                    if (!_chatCounters.TryGetValue(chatId, out state))
                    {
                        state = new State
                        {
                            LastTick = now,
                        };
                    }

                    var diff = (now - state.LastTick);
                    if (diff >= range)
                    {
                        state = new State
                        {
                            LastTick = now,
                        };
                    }

                    if (state.Counter == MaxRateForChatPerSecond)
                    {
                        SetWaitTime(range - diff);

                        result = false;
                    }
                    else
                    {
                        state.Counter++;
                    }
                }

                var chats = _chats.ToList();
                if (result)
                {
                    var range = TimeSpan.FromSeconds(1);

                    if (!_lastTick.HasValue)
                    {
                        _lastTick = now;
                    }
                    var lastTick = _lastTick.Value;

                    var diff = (now - lastTick);
                    if (diff >= range)
                    {
                        chats.Clear();
                    }

                    if (chats.Count == MaxRateForMultipleChatsPerSecond)
                    {
                        SetWaitTime(range - diff);

                        result = false;
                    }
                    else if (!_chats.Contains(chatId))
                    {
                        chats.Add(chatId);
                    }
                }

                State groupState = new();
                if (result)
                {
                    var range = TimeSpan.FromMinutes(1);

                    if (!_groupCounters.TryGetValue(chatId, out groupState))
                    {
                        groupState = new State
                        {
                            LastTick = now,
                        };
                    }

                    var diff = (now - groupState.LastTick);
                    if (diff >= range)
                    {
                        groupState = new State
                        {
                            LastTick = now,
                        };
                    }

                    if (groupState.Counter == MaxRateForGroupPerMinute)
                    {
                        SetWaitTime(range - diff);

                        result = false;
                    }
                    else
                    {
                        groupState.Counter++;
                    }
                }

                if (result)
                {
                    _chatCounters[chatId] = state;
                    _chats = chats;
                    _groupCounters[chatId] = groupState;
                }
            }

            waitTime = resultWaitTime;
            return result;
        }
    }
}
