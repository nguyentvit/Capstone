namespace Capstone.Application.Identity.Commands.Logout;

public class LogoutHandler : ICommandHandler<LogoutCommand, LogoutResult>
{
    public Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
