using Microsoft.AspNetCore.Http;

namespace Capstone.Domain.Common.ValueObjects;

public record FileVO
{
    public IFormFile Value { get; }
    private FileVO(IFormFile value) => Value = value;
    public static FileVO Of(IFormFile value)
    {
        if (value == null)
            throw new ArgumentException("File cannot be null.", nameof(value));
        
        if (value.Length == 0)
            throw new ArgumentException("File cannot be empty.", nameof(value));

        return new FileVO(value);
    }
}