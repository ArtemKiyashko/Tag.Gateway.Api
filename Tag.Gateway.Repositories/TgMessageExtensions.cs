using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Tag.Gateway.Repositories;

public static class TgMessageExtensions
{
    public static string GetSessionId(this Update update)
    =>  update.Type switch
        {
            UpdateType.Message => update.Message.Chat.Id.ToString(),
            UpdateType.EditedMessage => update.EditedMessage.Chat.Id.ToString(),
            UpdateType.MyChatMember => update.MyChatMember.Chat.Id.ToString(),
            _ => Guid.NewGuid().ToString(),
        };
}
