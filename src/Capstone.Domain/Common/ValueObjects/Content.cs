namespace Capstone.Domain.Common.ValueObjects;
public record Content
{
    public string Value { get; private set; }
    private Content(string value) => Value = value;
    public static Content Of(string value)
    {
        return new Content(value);
    }
}