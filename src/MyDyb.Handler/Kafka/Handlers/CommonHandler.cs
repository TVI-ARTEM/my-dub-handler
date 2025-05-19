using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using MyDyb.Handler.Clients.MlService.Api;
using MyDyb.Handler.Clients.MlService.Model;
using MyDyb.Handler.Kafka.Models;
using Segment = MyDyb.Handler.Clients.MlService.Model.Segment;

namespace MyDyb.Handler.Kafka.Handlers;

public class CommonHandler(
    SegmentsApi segmentsApi,
    ILogger<CommonHandler> logger) : IConsumerHandler<KafkaEvent>
{
    public async Task HandleMessage(ConsumeResult<string, KafkaEvent> message, CancellationToken cancellationToken)
    {
        switch (message.Message.Value.EventType)
        {
            case EventType.GenerateSegments:
                await HandleGenerateSegments(message.Message.Value, cancellationToken);
                break;
            case EventType.SynthesizeSegment:
                await HandleSynthesizeSegments(message.Message.Value, cancellationToken);
                break;
            case EventType.None:
            default:
                logger.LogWarning("Incorect eventType");
                break;
        }
    }

    private async Task HandleGenerateSegments(KafkaEvent message, CancellationToken cancellationToken)
    {
        var result = await segmentsApi.CreateSegmentsApiSegmentsGeneratePostAsync(
            new GenerateSegmentsRequest(mediaId: message.MediaId),
            cancellationToken: cancellationToken);
    }

    private async Task HandleSynthesizeSegments(KafkaEvent message, CancellationToken cancellationToken)
    {
        var result = await segmentsApi.CreateSegmentApiSegmentsSynthesizePostAsync(new SynthesizeSegmentRequest(
            mediaId: message.MediaId,
            segment: new Segment(
                id: message.Segment.Id,
                startMs: message.Segment.StartMs,
                endMs: message.Segment.EndMs,
                speaker: message.Segment.Speaker,
                transcribe: message.Segment.Transcribe,
                translationRu: message.Segment.TranslationRu,
                accentRu: message.Segment.AccentRu,
                audioMediaId: string.Empty
            )
        ), cancellationToken: cancellationToken);
    }
}