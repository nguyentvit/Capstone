namespace Capstone.Application.Common.DomainEvents;
public record SendEmailDomainEvent(string Email, string Otp) : IDomainEvent;