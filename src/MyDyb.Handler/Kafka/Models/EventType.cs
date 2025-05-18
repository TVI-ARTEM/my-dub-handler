namespace MyDyb.Handler.Kafka.Models;

public enum EventType
{
    None = 0,
    GenerateSegments = 1,
    SynthesizeSegment = 2,
}