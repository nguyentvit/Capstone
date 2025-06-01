namespace Capstone.Domain.Common.ValueObjects;
public record Image
{
    public string Url { get; } = default!;
    private Image(string url)
    {
        Url = url;
    }
    public static Image Of(string url)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new DomainException("Url is not a valid absolute URI");
        }

        return new Image(url);
    }
}