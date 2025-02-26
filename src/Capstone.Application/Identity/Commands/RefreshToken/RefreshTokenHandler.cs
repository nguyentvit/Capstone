namespace Capstone.Application.Identity.Commands.RefreshToken;
public class RefreshTokenHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
