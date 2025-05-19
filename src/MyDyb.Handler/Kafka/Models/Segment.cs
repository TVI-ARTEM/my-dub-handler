namespace MyDyb.Handler.Kafka.Models;

public class Segment
{
    public string Id { get; init; } = default!;
    public int StartMs { get; init; }
    public int EndMs { get; init; }
    public int Speaker { get; init; }
    public string Transcribe { get; init; } = string.Empty;
    public string TranslationRu { get; init; } = string.Empty;
    public string AccentRu { get; init; } = string.Empty;
    public string AudioMediaId { get; init; } = string.Empty;
}