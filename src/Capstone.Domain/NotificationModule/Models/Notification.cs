using Capstone.Domain.NotificationModule.Enums;
using Capstone.Domain.NotificationModule.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.NotificationModule.Models;
public class Notification : Aggregate<NotificationId>
{
    public UserId UserId { get; private set; } = default!;
    public UserId? SenderId { get; private set; }
    public Content Content { get; private set; } = default!;
    public Title Title { get; private set; } = default!;
    public NotificationType Type { get; private set; } = default!;
    public bool IsRead { get; private set; } = default!;
    private Notification() {}
    private Notification(NotificationId id, UserId userId, UserId? senderId, Content content, Title title, NotificationType type, bool isRead)
    {
        Id = id;
        UserId = userId;
        SenderId = senderId;
        Content = content;
        Title = title;
        Type = type;
        IsRead = isRead;
    }
    public static Notification Of(UserId userId, UserId? senderId, Content content, Title title, NotificationType type)
    {
        var notification = new Notification(
            NotificationId.Of(Guid.NewGuid()),
            userId,
            senderId,
            content,
            title,
            type,
            false
        );

        return notification;
    }
}