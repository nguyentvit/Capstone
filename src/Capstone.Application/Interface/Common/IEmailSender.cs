namespace Capstone.Application.Interface.Common.Services;
public interface IEmailSender
{
    Task SendEmailAsync(string Email, string Subject, string Message);
}