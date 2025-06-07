using Common.Library.Kafka.Consumer.Interfaces;
using Confluent.Kafka;
using MyDyb.Handler.Clients.MlService.Api;
using MyDyb.Handler.Clients.MlService.Model;
using MyDyb.Handler.Clients.ProjectService.Api;
using MyDyb.Handler.Clients.ProjectService.Model;
using MyDyb.Handler.Kafka.Models;
using Segment = MyDyb.Handler.Clients.MlService.Model.Segment;

namespace MyDyb.Handler.Kafka.Handlers;

public class CommonHandler(
    ISegmentsApi segmentsApi,
    IProjectApi projectApi,
    ILogger<CommonHandler> logger) : IConsumerHandler<KafkaEvent>
{
    public async Task HandleMessage(ConsumeResult<string, KafkaEvent> message, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Start processing {message.Message.Value.ProjectId}");
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
                logger.LogWarning("Incorrect eventType");
                break;
        }
    }

    private async Task HandleGenerateSegments(KafkaEvent message, CancellationToken cancellationToken)
    {
        var result = await segmentsApi.CreateSegmentsApiSegmentsGeneratePostAsync(
            new GenerateSegmentsRequest(mediaId: message.MediaId),
            cancellationToken: cancellationToken);

        await projectApi.ApiProjectsSegmentsPostAsync(
            updateSegmentsRequest: new UpdateSegmentsRequest(
                projectId: message.ProjectId,
                segments: result.Segments.Select(it => new SegmentInfo(
                    id: it.Id,
                    startMs: it.StartMs,
                    endMs: it.EndMs,
                    speaker: it.Speaker,
                    transcribe: it.Transcribe,
                    translationRu: it.TranslationRu,
                    accentRu: it.AccentRu,
                    audioMediaId: it.AudioMediaId,
                    externalRefId: it.ExternalRefId,
                    trueDub: it.TrueDub
                )).ToList()),
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
                externalRefId: message.Segment.ExternalRefId
            )
        ), cancellationToken: cancellationToken);

        await projectApi.ApiProjectsIdSegmentPutAsync(
            message.ProjectId,
            updateSegmentRequest: new UpdateSegmentRequest(segment: new SegmentInfo(
                id: result.Segment.Id,
                startMs: result.Segment.StartMs,
                endMs: result.Segment.EndMs,
                speaker: result.Segment.Speaker,
                transcribe: result.Segment.Transcribe,
                translationRu: result.Segment.TranslationRu,
                accentRu: result.Segment.AccentRu,
                audioMediaId: result.Segment.AudioMediaId,
                externalRefId: result.Segment.ExternalRefId,
                trueDub: result.Segment.TrueDub
            )),
            cancellationToken: cancellationToken);
    }
}