namespace Capstone.Application.ExamTemplateModule.Commands.CreateExamTemplate
{
    public record CreateExamTemplateCommand(Guid UserId, string Role, Guid SubjectId, string Title, string Description, int Duration) : ICommand<CreateExamTemplateResult>;
    public record CreateExamTemplateResult(Guid Id);
    public class CreateExamTemplateCommandValidator : AbstractValidator<CreateExamTemplateCommand>
    {
        public CreateExamTemplateCommandValidator()
        {

        }
    }
}
