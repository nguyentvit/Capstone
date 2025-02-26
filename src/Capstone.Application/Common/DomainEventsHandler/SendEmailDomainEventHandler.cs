using Capstone.Application.Common.DomainEvents;

namespace Capstone.Application.Common.DomainEventsHandler;
public class SendEmailDomainEventHandler() : IDomainEventHandler<SendEmailDomainEvent>
{
    public Task Handle(SendEmailDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
