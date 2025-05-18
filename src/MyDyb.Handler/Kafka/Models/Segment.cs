namespace MyDyb.Handler.Kafka.Models;

public class Segment
{
    public string Id { get; init; } = default!;
    public long StartMs { get; init; }
    public long EndMs { get; init; }
    public long Speaker { get; init; }
    public string Transcribe { get; init; } = string.Empty;
    public string TranslationRu { get; init; } = string.Empty;
    public string AccentRu { get; init; } = string.Empty;
    public string AudioMediaId { get; init; } = string.Empty;
}