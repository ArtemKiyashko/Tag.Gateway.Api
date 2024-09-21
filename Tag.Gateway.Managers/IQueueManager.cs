namespace Tag.Gateway.Managers;

public interface IQueueManager
{
    Task PostMessageAsync<T>(T message);
}
