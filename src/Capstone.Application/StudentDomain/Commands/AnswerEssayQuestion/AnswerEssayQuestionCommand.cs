namespace Capstone.Application.StudentDomain.Commands.AnswerEssayQuestion
{
    public record AnswerEssayQuestionCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId, string Answer) : ICommand<AnswerEssayQuestionResult>;
    public record AnswerEssayQuestionResult(bool IsSuccess);
    public class AnswerEssayQuestionCommandValidator : AbstractValidator<AnswerEssayQuestionCommand>
    {
        public AnswerEssayQuestionCommandValidator()
        {

        }
    }
}
