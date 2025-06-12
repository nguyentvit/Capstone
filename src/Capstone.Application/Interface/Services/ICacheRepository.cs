namespace Capstone.Application.Interface.Services
{
    public interface ICacheRepository
    {
        Task<bool> AddConnectionId(Guid UserId, string connectionId);
    }
}
