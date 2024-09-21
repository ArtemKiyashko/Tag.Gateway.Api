namespace Tag.Gateway.Api;

public class ServiceBusOptions
{
    public string TopicName { get; set; } = "tgmessages";
    public string? ServiceBusNamespace { get; set; }
    public string? ServiceBusConnectionString { get; set; }
}
