using Capstone.Application.Interface.Common.Services;
using FluentEmail.Core;

namespace Capstone.Infrastructure.Services.Common;
public class EmailSender(IFluentEmail fluentEmail) : IEmailSender
{
    public async Task SendEmailAsync(string Email, string Subject, string Message)
    {
        await fluentEmail
            .To(Email)
            .Subject(Subject)
            .Body(Message)
            .SendAsync();
    }
}
