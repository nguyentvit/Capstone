namespace Capstone.Domain.UserAccess.ValueObjects;

public record UserName
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    public string Value { get; }
    private UserName(string value) => Value = value;
    public static UserName Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);

        return new UserName(value);
    }
}