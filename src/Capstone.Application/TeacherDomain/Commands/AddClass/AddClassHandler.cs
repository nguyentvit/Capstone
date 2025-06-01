using Capstone.Domain.SubjectDomain.ValueObjects;
using Capstone.Domain.ClassDomain.Models;
using Capstone.Domain.ClassDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Commands.AddClass
{
    public class AddClassHandler(IApplicationDbContext dbContext) : ICommandHandler<AddClassCommand, AddClassResult>
    {
        public async Task<AddClassResult> Handle(AddClassCommand command, CancellationToken cancellationToken)
        {
            var subjectId = SubjectId.Of(command.SubjectId);
            var userId = UserId.Of(command.UserId);

            var teacherSubject = await dbContext.TeacherSubjects
                                            .Where(s => s.Id == subjectId && s.OwnerId == userId)
                                            .FirstOrDefaultAsync(cancellationToken);

            if (teacherSubject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            var newClass = Class.Of(
                subjectId,
                ClassName.Of(command.ClassName)
                );


            teacherSubject.AddClass(newClass.Id);
            dbContext.Classes.Add(newClass);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddClassResult(newClass.Id.Value);
        }
    }
}
