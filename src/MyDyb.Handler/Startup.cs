using Common.Library.Kafka.Common.Extensions;
using Common.Library.Kafka.Consumer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using MyDyb.Handler.Clients.MlService.Api;
using MyDyb.Handler.Clients.MlService.Client;
using MyDyb.Handler.Clients.ProjectService.Api;
using MyDyb.Handler.Configures;
using MyDyb.Handler.Kafka.Handlers;
using MyDyb.Handler.Kafka.Models;

namespace MyDyb.Handler;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<MlServiceOptions>(configuration.GetSection(nameof(MlServiceOptions)));
        services.Configure<ProjectServiceOptions>(configuration.GetSection(nameof(ProjectServiceOptions)));
        services.AddCommonKafka(configuration);

        services.AddSingleton<ISegmentsApi>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<MlServiceOptions>>();

            return new SegmentsApi(new Configuration
            {
                BasePath = config.Value.BaseUrl,
                Timeout = 4800000,
            });
        });

        services.AddSingleton<IProjectApi>(provider =>
        {
            var config = provider.GetRequiredService<IOptions<ProjectServiceOptions>>();

            return new ProjectApi(config.Value.BaseUrl);
        });

        services.AddConsumerHandler<KafkaEvent, KafkaOptions, CommonHandler>(configuration);
    }

    public void Configure(IHostEnvironment environment, IApplicationBuilder app)
    {
    }
}