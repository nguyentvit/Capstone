using Capstone.Application.Common.DomainEvents;
using Capstone.Application.Interface.Common.Services;

namespace Capstone.Application.Common.DomainEventsHandler;
public class SendEmailDomainEventHandler(IEmailSender emailSender) : IDomainEventHandler<SendEmailDomainEvent>
{
    public async Task Handle(SendEmailDomainEvent notification, CancellationToken cancellationToken)
    {
        await emailSender.SendEmailAsync(notification.Email, "Verify your OTP", $"Your OTP is {notification.Otp}");
    }
}
