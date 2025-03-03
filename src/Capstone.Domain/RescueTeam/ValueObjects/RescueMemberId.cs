namespace Capstone.Domain.RescueTeam.ValueObjects;
public record RescueMemberId
{
    public Guid Value { get; }
    private RescueMemberId(Guid value) => Value = value;
    public static RescueMemberId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("UserId cannot be empty.");
        }

        return new RescueMemberId(value);
    }
}