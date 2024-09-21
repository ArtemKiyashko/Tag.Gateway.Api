namespace Tag.Gateway.Repositories;

internal interface IQueueRepository
{
    Task PostMessageAsync<T>(T message);
}
