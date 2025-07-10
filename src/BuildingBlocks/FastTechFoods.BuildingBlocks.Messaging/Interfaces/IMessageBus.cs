namespace FastTechFoods.BuildingBlocks.Messaging.Interfaces;

public interface IMessageBus
{
    Task PublishAsync<T>(string topicOrQueue, T message);
}
