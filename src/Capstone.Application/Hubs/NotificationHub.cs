using Capstone.Application.Interface.Services;
using Capstone.Domain.ExamSessionModule.Enums;
using Microsoft.AspNetCore.SignalR;

namespace Capstone.Application.Hubs
{
    public class NotificationHub(ICacheRepository cache) : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public async Task AddNewUser(Guid UserId)
        {
            var connectionId = Context.ConnectionId;
            await cache.AddConnectionId(UserId, connectionId);
        }
        public async Task SendStatus(Guid ParticipantId, ActionType action)
        {

        }
    }
}
