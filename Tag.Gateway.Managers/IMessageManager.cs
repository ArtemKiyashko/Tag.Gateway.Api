using Telegram.Bot.Types;

namespace Tag.Gateway.Managers;

public interface IMessageManager
{
    Task PostMessageAsync(Update message);
}
