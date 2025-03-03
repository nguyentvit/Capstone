namespace Capstone.Domain.Common.ValueObjects;
public record Image
{
    public string Url { get; } = default!;
    public ImageFormat Format { get; } = default!;
    private Image(string url, ImageFormat format)
    {
        Url = url;
        Format = format;
    }
    public static Image Of(string url, ImageFormat format)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new DomainException("Url is not a valid absolute URI");
        }

        return new Image(url, format);
    }
}