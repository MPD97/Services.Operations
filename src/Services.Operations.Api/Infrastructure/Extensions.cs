using System;
using Convey;
using Convey.Auth;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Convey.CQRS.Queries;
using Convey.Discovery.Consul;
using Convey.Docs.Swagger;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Convey.MessageBrokers;
using Convey.MessageBrokers.CQRS;
using Convey.MessageBrokers.RabbitMQ;
using Convey.Metrics.AppMetrics;
using Convey.Persistence.MongoDB;
using Convey.Persistence.Redis;
using Convey.Security;
using Convey.Tracing.Jaeger;
using Convey.Tracing.Jaeger.RabbitMQ;
using Convey.WebApi;
using Convey.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Thesis.Services.Operations.Api.Decorators;
using Thesis.Services.Operations.Api.Events;
using Thesis.Services.Operations.Api.Events.External;
using Thesis.Services.Operations.Api.Handlers;
using Thesis.Services.Operations.Api.Services;
using Thesis.Services.Operations.Api.Types;

namespace Thesis.Services.Operations.Api.Infrastructure
{
    public static class Extensions
    {
        public static string ToUserGroup(this Guid userId) => userId.ToString("N").ToUserGroup();
        public static string ToUserGroup(this string userId) => $"users:{userId}";

        public static CorrelationContext GetCorrelationContext(this ICorrelationContextAccessor accessor)
        {
            if (accessor.CorrelationContext is null)
            {
                return null;
            }

            var payload = JsonConvert.SerializeObject(accessor.CorrelationContext);

            return string.IsNullOrWhiteSpace(payload)
                ? null
                : JsonConvert.DeserializeObject<CorrelationContext>(payload);
        }

        public static IConveyBuilder AddInfrastructure(this IConveyBuilder builder)
        {
            var requestsOptions = builder.GetOptions<RequestsOptions>("requests");
            builder.Services.AddSingleton(requestsOptions);
            builder.Services.AddTransient<ICommandHandler<ICommand>, GenericCommandHandler<ICommand>>()
                /*.AddTransient<IEventHandler<IEvent>, GenericEventHandler<IEvent>>()*/
                .AddTransient<IEventHandler<IRejectedEvent>, GenericRejectedEventHandler<IRejectedEvent>>()
                .AddTransient<IHubService, HubService>()
                .AddTransient<IHubWrapper, HubWrapper>()
                .AddSingleton<IOperationsService, OperationsService>();
            builder.Services.Decorate(typeof(IEventHandler<>), typeof(EventPublisherDecorator<>));
            builder.Services.AddGrpc();

            return builder
                .AddErrorHandler<ExceptionToResponseMapper>()
                .AddJwt()
                .AddQueryHandlers()
                .AddHttpClient()
                .AddConsul()
                .AddFabio()
                .AddRabbitMq(plugins: p => p.AddJaegerRabbitMqPlugin())
                .AddMongo()
                .AddRedis()
                .AddMetrics()
                .AddJaeger()
                .AddRedis()
                .AddSignalR()
                .AddWebApiSwaggerDocs()
                .AddSecurity();
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseErrorHandler()
                .UseSwaggerDocs()
                .UseJaeger()
                .UseConvey()
                .UseMetrics()
                .UseStaticFiles()
                .UseRabbitMq()
                .SubscribeEvent<RunCancelled>()
                .SubscribeEvent<RunCompleted>()
                .SubscribeEvent<RunCreated>()
                .SubscribeEvent<RunPointCompleted>();

            return app;
        }

        private static IConveyBuilder AddSignalR(this IConveyBuilder builder)
        {
            var options = builder.GetOptions<SignalrOptions>("signalR");
            builder.Services.AddSingleton(options);
            var signalR = builder.Services.AddSignalR();
            if (!options.Backplane.Equals("redis", StringComparison.InvariantCultureIgnoreCase))
            {
                return builder;
            }

            var redisOptions = builder.GetOptions<RedisOptions>("redis");
            signalR.AddRedis(redisOptions.ConnectionString);

            return builder;
        }
    }
}