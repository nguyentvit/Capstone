using System.Text.RegularExpressions;

namespace Capstone.Domain.Common.ValueObjects;

public record PhoneNumber
{
    private const string PatternPhoneNumber = @"^\+?[0-9\s\-()]*$";
    private const int MaxLength = 15;
    private const int MinLength = 10;
    public string Value { get; }
    private PhoneNumber(string value) => Value = value;
    public static PhoneNumber Of(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MaxLength);
        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MinLength);
        if (!Regex.IsMatch(value, PatternPhoneNumber))
        {
            throw new DomainException("Phone number is not in a valid format.");
        }
        return new PhoneNumber(value);
    }
}