namespace Capstone.Application.UserAccess.Commands.UpdateUser
{
    public class UpdateUserHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
    {
        public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.Id);

            var user = await dbContext.AppUsers.FindAsync([userId], cancellationToken) ?? throw new UserNotFoundException(userId.Value);

            UpdateUserWithNewValues(user, command);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateUserResult(true);
        }
        private static void UpdateUserWithNewValues(User user, UpdateUserCommand command)
        {
            user.UpdateUser(
                command.UserName != null ? UserName.Of(command.UserName) : null,
                command.Email != null ? Email.Of(command.Email) : null,
                command.PhoneNumber != null ? PhoneNumber.Of(command.PhoneNumber) : null,
                command.Avatar != null ? FileVO.Of(command.Avatar) : null
                );
        }
    }
}
