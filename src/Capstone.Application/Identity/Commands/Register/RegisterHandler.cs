namespace Capstone.Application.Identity.Commands.Register;
public class RegisterHandler : ICommandHandler<RegisterCommand, RegisterResult>
{
    public Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
