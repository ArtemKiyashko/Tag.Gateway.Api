namespace Tag.Gateway.Api;

public class QueuesOptions
{
    public string QueueName { get; set; } = "tgmessages";
    public Uri? QueueAccountUri { get; set; }
    public string? QueueAccountConnectionString { get; set; }
}
