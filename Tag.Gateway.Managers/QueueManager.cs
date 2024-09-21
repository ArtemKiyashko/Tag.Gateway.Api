
using Tag.Gateway.Repositories;

namespace Tag.Gateway.Managers;

internal class QueueManager(IQueueRepository queueRepository) : IQueueManager
{
    private readonly IQueueRepository _queueRepository = queueRepository;

    public Task PostMessageAsync<T>(T message) => _queueRepository.PostMessageAsync(message);
}
