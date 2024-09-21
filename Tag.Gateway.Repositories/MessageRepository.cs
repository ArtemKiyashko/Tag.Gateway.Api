using System.Runtime.CompilerServices;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using Telegram.Bot.Types;

[assembly: InternalsVisibleTo("Tag.Gateway.Managers")]
namespace Tag.Gateway.Repositories;

internal class MessageRepository(ServiceBusSender serviceBusSender) : IMessageRepository
{
    private readonly ServiceBusSender _serviceBusSender = serviceBusSender;

    public async Task PostMessageAsync(Update message)
    {
        var sessionId = message.GetSessionId();
        var sbMessage = new ServiceBusMessage(JsonConvert.SerializeObject(message))
        {
            SessionId = sessionId
        };

        await _serviceBusSender.SendMessageAsync(sbMessage);
    }
}
