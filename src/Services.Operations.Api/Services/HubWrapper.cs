using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Thesis.Services.Operations.Api.Hubs;
using Thesis.Services.Operations.Api.Infrastructure;

namespace Thesis.Services.Operations.Api.Services
{
    public class HubWrapper : IHubWrapper
    {
        private readonly IHubContext<ThesisHub> _hubContext;

        public HubWrapper(IHubContext<ThesisHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(string userId, string message, object data)
            => await _hubContext.Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

        public async Task PublishToAllAsync(string message, object data)
            => await _hubContext.Clients.All.SendAsync(message, data);
    }
}