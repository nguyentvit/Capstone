using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Capstone.Application.AdminDomain.Commands.AddTeacher
{
    public class AddTeacherHandler(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : ICommandHandler<AddTeacherCommand, AddTeacherResult>
    {
        public async Task<AddTeacherResult> Handle(AddTeacherCommand command, CancellationToken cancellationToken)
        {
            if (await userManager.FindByNameAsync(command.TeacherId) != null)
                throw new TeacherDuplicateExpcetion(command.TeacherId);

            var teacher = AddTeacherCommandToTeacher(command);
            var teacherAccount = new ApplicationUser
            {
                UserName = teacher.TeacherId.Value,
                EmailConfirmed = true,
                UserId = teacher.Id.Value
            };

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = await userManager.CreateAsync(teacherAccount, teacherAccount.UserName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new TeacherBadRequestException(errors);
            }

            dbContext.Teachers.Add(teacher);

            await userManager.AddToRoleAsync(teacherAccount, RoleConstant.TEACHER);
            await dbContext.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return new AddTeacherResult(teacher.Id.Value);
        }

        private static Teacher AddTeacherCommandToTeacher(AddTeacherCommand command)
        {
            return Teacher.Of(
                UserName.Of(command.UserName),
                (command.Email != null) ? Email.Of(command.Email) : null,
                (command.PhoneNumber != null) ? PhoneNumber.Of(command.PhoneNumber) : null,
                (command.Avatar != null) ? FileVO.Of(command.Avatar) : null,
                TeacherId.Of(command.TeacherId)
            );
        }
    }
}
