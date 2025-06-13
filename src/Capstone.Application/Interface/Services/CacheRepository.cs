using Capstone.Application.Hubs;
using Microsoft.Extensions.Caching.Distributed;

namespace Capstone.Application.Interface.Services
{
    public class CacheRepository(IDistributedCache cache, IApplicationDbContext dbContext) : ICacheRepository
    {
        public async Task<bool> AddConnectionId(Guid UserId, string connectionId)
        {
            var user = new UserResponseDto(UserId, connectionId);
            await cache.SetStringAsync(UserId.ToString(), System.Text.Json.JsonSerializer.Serialize(user));
            return true;
        }

        public async Task<UserResponseDto> GetUser(Guid userId)
        {
            var cachedUser = await cache.GetStringAsync(userId.ToString());
            if (!string.IsNullOrEmpty(cachedUser))
                return System.Text.Json.JsonSerializer.Deserialize<UserResponseDto>(cachedUser)!;

            return new UserResponseDto(userId, string.Empty);
        }

        public async Task RemoveConnectionId(string connectionId)
        {
            var userIds = await dbContext.ApplicationUsers.Select(u => u.UserId).ToListAsync();
            foreach (var userId in userIds)
            {
                var cachedUser = await cache.GetStringAsync(userId.ToString());
                if (!string.IsNullOrEmpty(cachedUser))
                {
                    var user = System.Text.Json.JsonSerializer.Deserialize<UserResponseDto>(cachedUser)!;
                    if (user.ConnectionId == connectionId)
                    {
                        await cache.RemoveAsync(user.Id.ToString());
                    }
                }
            }
        }
    }
}
