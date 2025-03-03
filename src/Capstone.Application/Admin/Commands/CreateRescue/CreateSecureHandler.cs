
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.RescueTeam.Models;
using Capstone.Domain.RescueTeam.ValueObjects;

namespace Capstone.Application.Admin.Commands.CreateRescue;
public class CreateRescueHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateRescueCommand, CreateRescueResult>
{
    public async Task<CreateRescueResult> Handle(CreateRescueCommand command, CancellationToken cancellationToken)
    {
        var userId = UserId.Of(command.ManagerId);
        var user = await dbContext.AppUsers.FindAsync([userId], cancellationToken);
        if (user == null)
            throw new UserNotFoundException(userId.Value);

        var Rescue = ToRescue(command);
        dbContext.Rescues.Add(Rescue);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateRescueResult(Rescue.Id.Value);
    }
    private static Rescue ToRescue(CreateRescueCommand command)
    {
        return Rescue.Of(
            RescueName.Of(command.RescueName),
            PhoneNumber.Of(command.Phone),
            Address.Of(command.District, command.Ward, command.Province),
            Coordinates.Of(command.Latitude, command.Longitude),
            UserId.Of(command.ManagerId)
        );
    }
}
