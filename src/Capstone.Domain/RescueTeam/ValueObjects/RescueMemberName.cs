namespace Capstone.Domain.RescueTeam.ValueObjects;
public record RescueMemberName
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    public string Value { get; }
    private RescueMemberName(string value) => Value = value;
    public static RescueMemberName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);

        return new RescueMemberName(value);
    }
}