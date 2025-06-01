using Capstone.Domain.ExamTemplateModule.Models;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.SubjectDomain.ValueObjects;

namespace Capstone.Application.ExamTemplateModule.Commands.CreateExamTemplate
{
    public class CreateExamTemplateHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateExamTemplateCommand, CreateExamTemplateResult>
    {
        public async Task<CreateExamTemplateResult> Handle(CreateExamTemplateCommand command, CancellationToken cancellationToken)
        {
            var subjectId = SubjectId.Of(command.SubjectId);
            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == subjectId)
                                         .FirstOrDefaultAsync(cancellationToken);

            if (subject == null)
                throw new SubjectNotFoundException(subjectId.Value);

            var userId = UserId.Of(command.UserId);

            SubjectExtention.CheckRole(subject, userId, command.Role);

            var examTemplate = CreateExamTemplateCommandToExamTemplate(command);
            dbContext.ExamTemplates.Add(examTemplate);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateExamTemplateResult(examTemplate.Id.Value);
        }
        private static ExamTemplate CreateExamTemplateCommandToExamTemplate(CreateExamTemplateCommand command)
        {
            return ExamTemplate.Of(
                SubjectId.Of(command.SubjectId),
                ExamTemplateTitle.Of(command.Title),
                ExamTemplateDescription.Of(command.Description),
                ExamTemplateDuration.Of(command.Duration)
                );
        }
    }
}
