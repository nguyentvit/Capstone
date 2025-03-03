namespace Capstone.Domain.RescueTeam.ValueObjects;
public record RescueId
{
    public Guid Value { get; }
    private RescueId(Guid value) => Value = value;
    public static RescueId Of(Guid value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value == Guid.Empty)
        {
            throw new DomainException("UserId cannot be empty.");
        }

        return new RescueId(value);
    }
}