namespace MyDyb.Handler.Kafka.Models;

public class KafkaEvent
{
    public long ProjectId { get; init; }
    public EventType EventType { get; init; }
    public Segment Segment { get; init; } = new();
    public string MediaId { get; init; } = null!;
}