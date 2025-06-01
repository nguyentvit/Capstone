namespace Capstone.Domain.NotificationModule.ValueObjects;
public record Title
{
    public string Value { get; }
    private Title(string value) => Value = value;
    public static Title Of(string value)
    {
        return new Title(value);
    }
}