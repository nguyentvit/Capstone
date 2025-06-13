using Capstone.Application.Interface.Services;
using Capstone.Domain.ExamSessionModule.Enums;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Capstone.Application.Hubs
{
    public class NotificationHub(ICacheRepository cache, IApplicationDbContext dbContext, ILogger<NotificationHub> logger) : Hub
    {
        public override Task OnConnectedAsync()
        {
            logger.LogInformation("OnConnectedAsync");
            return base.OnConnectedAsync();
        }
        public async Task AddNewUser(Guid UserId)
        {
            logger.LogInformation("AddNewUser " + UserId.ToString());
            var connectionId = Context.ConnectionId;
            await cache.AddConnectionId(UserId, connectionId);
        }
        public async Task SendStatus(Guid PId, ActionType action)
        {
            var participantId = ParticipantId.Of(PId);

            var ownerId = await dbContext.ExamSessions
                                         .AsNoTracking()
                                         .Where(t => t.Participants.Any(p => p.Id == participantId))
                                         .Select(t => t.UserId.Value)
                                         .FirstOrDefaultAsync();

            var user = await cache.GetUser(Guid.NewGuid());

            await Clients.Client(user.ConnectionId).SendAsync("ReceiveStatus", new { PId, action });
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            await cache.RemoveConnectionId(connectionId);
        }
    }
}
