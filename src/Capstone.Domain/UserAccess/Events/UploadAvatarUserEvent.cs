using Capstone.Domain.UserAccess.Models;

namespace Capstone.Domain.UserAccess.Events
{
    public record UploadAvatarUserEvent(User User, FileVO Avatar) : IDomainEvent;
}
