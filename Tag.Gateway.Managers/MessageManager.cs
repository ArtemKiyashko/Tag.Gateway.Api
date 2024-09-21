
using Tag.Gateway.Repositories;
using Telegram.Bot.Types;

namespace Tag.Gateway.Managers;

internal class MessageManager(IMessageRepository messageRepository) : IMessageManager
{
    private readonly IMessageRepository _messageRepository = messageRepository;

    public Task PostMessageAsync(Update message) => _messageRepository.PostMessageAsync(message);
}
