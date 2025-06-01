using BuildingBlocks.Interfaces;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.UserAccess.Events;

namespace Capstone.Application.Common.DomainEventsHandler
{
    public class UploadAvatarUserEventHandler(IS3Service s3Service) : IDomainEventHandler<UploadAvatarUserEvent>
    {
        public async Task Handle(UploadAvatarUserEvent notification, CancellationToken cancellationToken)
        {
            var avatar = notification.Avatar;
            string avatarUrl = await s3Service.UploadFileAsync(avatar.Value);

            Image avatarImage = Image.Of(avatarUrl);
            notification.User.UpdateAvatar(avatarImage);
        }
    }
}
