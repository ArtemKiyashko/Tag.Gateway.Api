using Telegram.Bot.Types;

namespace Tag.Gateway.Repositories;

internal interface IMessageRepository
{
    Task PostMessageAsync(Update message);
}
