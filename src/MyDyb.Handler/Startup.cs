using Common.Library.Kafka.Common.Extensions;
using Common.Library.Kafka.Consumer.Extensions;
using Microsoft.AspNetCore.Builder;
using MyDyb.Handler.Kafka.Handlers;
using MyDyb.Handler.Kafka.Models;
using MyDyb.Handler.Kafka.Options;

namespace MyDyb.Handler;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCommonKafka(configuration);
        
        services.AddConsumerHandler<KafkaEvent, KafkaOptions, CommonHandler>(configuration);
        
        

    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {
    }
}