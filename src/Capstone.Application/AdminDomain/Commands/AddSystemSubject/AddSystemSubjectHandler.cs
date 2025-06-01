using Capstone.Domain.SubjectDomain.Models;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.AdminDomain.Commands.AddSystemSubject
{
    public class AddSystemSubjectHandler(IApplicationDbContext dbContext) : ICommandHandler<AddSystemSubjectCommand, AddSystemSubjectResult>
    {
        public async Task<AddSystemSubjectResult> Handle(AddSystemSubjectCommand command, CancellationToken cancellationToken)
        {
            var systemSubject = AddSystemSubjectCommandToSystemSubject(command);

            dbContext.SystemSubjects.Add(systemSubject);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddSystemSubjectResult(systemSubject.Id.Value);
            
        }
        private static SystemSubject AddSystemSubjectCommandToSystemSubject(AddSystemSubjectCommand command)
        {
            return SystemSubject.Of(
                SubjectName.Of(command.SubjectName)
                );
        }
    }
}
