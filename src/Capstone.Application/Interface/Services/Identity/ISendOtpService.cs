namespace Capstone.Application.Interface.Services.Identity;

public interface ISendOtpService
{
    Task SetCacheReponseAsync(string email, object response, TimeSpan timeOut);
    Task<string> GetCachedReponseAsync(string email);
    Task Remove(string email);
}