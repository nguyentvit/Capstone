namespace Capstone.Application.ExamDomain.Commands.GenerateExamVersions
{
    public record GenerateExamVersionsCommand(Guid UserId, Guid ExamId, int Count, int OrderQuestion, bool IsAnswerShuffled) : ICommand<GenerateExamVersionsResult>;
    public record GenerateExamVersionsResult(bool IsSuccess);
    public class GenerateExamVersionsCommandValidator : AbstractValidator<GenerateExamVersionsCommand>
    {
        public GenerateExamVersionsCommandValidator()
        {

        }
    }
}
