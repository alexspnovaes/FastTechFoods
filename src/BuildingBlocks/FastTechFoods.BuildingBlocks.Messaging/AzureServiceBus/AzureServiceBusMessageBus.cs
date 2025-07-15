using System.Text.Json;
using Azure.Messaging.ServiceBus;
using FastTechFoods.BuildingBlocks.Messaging.Interfaces;

namespace FastTechFoods.BuildingBlocks.Messaging.AzureServiceBus;

public class AzureServiceBusMessageBus : IMessageBus
{
    private readonly ServiceBusClient _client;

    public AzureServiceBusMessageBus(string connectionString)
    {
        _client = new ServiceBusClient(connectionString);
    }

    public async Task PublishAsync<T>(string queueOrTopic, T message)
    {
        var sender = _client.CreateSender(queueOrTopic);
        var json = JsonSerializer.Serialize(message);
        var sbMessage = new ServiceBusMessage(json);
        await sender.SendMessageAsync(sbMessage);
    }
}
