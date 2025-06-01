namespace Capstone.Domain.Common.ValueObjects;

public record Date
{
    public DateTime Value { get; }
    private Date(DateTime value) => Value = value;
    public static Date Of(DateTime value)
    {
        return new Date(value);
    }
}