namespace Capstone.Application.ExamDomain.Commands.CreateExam
{
    public record CreateExamCommand(Guid UserId, Guid ExamTemplateId, int Duration, string Title) : ICommand<CreateExamResult>;
    public record CreateExamResult(Guid Id);
    public class CreateExamCommandValidator : AbstractValidator<CreateExamCommand>
    {
        public CreateExamCommandValidator()
        {

        }
    }
}
