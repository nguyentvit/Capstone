using Capstone.Domain.SubjectDomain.Models;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Commands.AddTeacherSubject
{
    public class AddTeacherSubjectHandler(IApplicationDbContext dbContext) : ICommandHandler<AddTeacherSubjectCommand, AddTeacherSubjectResult>
    {
        public async Task<AddTeacherSubjectResult> Handle(AddTeacherSubjectCommand command, CancellationToken cancellationToken)
        {
            var teacherSubject = AddTeacherSubjectCommandToTeacherSubject(command);

            dbContext.TeacherSubjects.Add(teacherSubject);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddTeacherSubjectResult(teacherSubject.Id.Value);
        }
        private static TeacherSubject AddTeacherSubjectCommandToTeacherSubject(AddTeacherSubjectCommand command)
        {
            return TeacherSubject.Of(
                SubjectName.Of(command.SubjectName),
                UserId.Of(command.UserId)
                );
        }
    }
}
