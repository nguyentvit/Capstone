namespace Capstone.Domain.RescueTeam.ValueObjects;
public record Passport
{
    public string Value { get; }
    private Passport(string value) => Value = value;
    public static Passport Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return new Passport(value);
    }
}