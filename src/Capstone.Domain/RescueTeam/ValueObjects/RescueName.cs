namespace Capstone.Domain.RescueTeam.ValueObjects;
public record RescueName
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    public string Value { get; }
    private RescueName(string value) => Value = value;
    public static RescueName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);

        return new RescueName(value);
    }
}