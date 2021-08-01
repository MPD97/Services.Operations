using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Events;
using Convey.Logging;
using Convey.Secrets.Vault;
using Convey.Types;
using Convey.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Thesis.Services.Operations.Api.Application;
using Thesis.Services.Operations.Api.Hubs;
using Thesis.Services.Operations.Api.Infrastructure;
using Thesis.Services.Operations.Api.Queries;
using Thesis.Services.Operations.Api.Services;

namespace Thesis.Services.Operations.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
            => await WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services
                    .AddConvey()
                    .AddWebApi()
                    .AddApplication()
                    .AddInfrastructure()
                    .Build())
                .Configure(app => app
                    .UseInfrastructure()
                    .UseEndpoints(endpoints => endpoints
                        .Get("", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetService<AppOptions>().Name))
                        .Get<GetOperation>("operations/{operationId}", async (query, ctx) =>
                        {
                            var operation = await ctx.RequestServices.GetService<IOperationsService>()
                                .GetAsync(query.OperationId);
                            if (operation is null)
                            {
                                await ctx.Response.NotFound();
                                return;
                            }

                            await ctx.Response.WriteJsonAsync(operation);
                        }))
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<ThesisHub>("/hub");
                        endpoints.MapGrpcService<GrpcServiceHost>();
                    }))
                .UseLogging()
                .Build()
                .RunAsync();
    }
}