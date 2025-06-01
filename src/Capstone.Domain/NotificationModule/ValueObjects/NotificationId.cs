namespace Capstone.Domain.NotificationModule.ValueObjects;
public record NotificationId
{
    public Guid Value { get; }
    private NotificationId(Guid value) => Value = value;
    public static NotificationId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("NotificationId cannot be empty.");
        }

        return new NotificationId(value);
    }
}