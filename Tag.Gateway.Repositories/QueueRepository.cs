using System.Runtime.CompilerServices;
using Azure.Storage.Queues;
using Newtonsoft.Json;

[assembly: InternalsVisibleTo("Tag.Gateway.Managers")]
namespace Tag.Gateway.Repositories;

internal class QueueRepository : IQueueRepository
{
    private readonly QueueClient _queueClient;

    public QueueRepository(QueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public Task PostMessageAsync<T>(T message) => 
        _queueClient.SendMessageAsync(JsonConvert.SerializeObject(message));
}
