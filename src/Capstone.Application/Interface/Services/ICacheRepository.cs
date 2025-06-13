using Capstone.Application.Hubs;

namespace Capstone.Application.Interface.Services
{
    public interface ICacheRepository
    {
        Task<bool> AddConnectionId(Guid UserId, string connectionId);
        Task<UserResponseDto> GetUser(Guid userId);
        Task RemoveConnectionId(string connectionId);
    }
}
