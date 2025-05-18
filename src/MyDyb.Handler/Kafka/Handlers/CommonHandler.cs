using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using MyDyb.Handler.Kafka.Models;

namespace MyDyb.Handler.Kafka.Handlers;

public class CommonHandler : IConsumerHandler<KafkaEvent>
{
    public Task HandleMessage(ConsumeResult<string, KafkaEvent> message, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}