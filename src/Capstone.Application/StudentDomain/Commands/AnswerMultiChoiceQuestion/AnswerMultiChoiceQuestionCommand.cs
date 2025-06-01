namespace Capstone.Application.StudentDomain.Commands.AnswerMultiChoiceQuestion
{
    public record AnswerMultiChoiceQuestionCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId, List<Guid> Answer) : ICommand<AnswerMultiChoiceQuestionResult>;
    public record AnswerMultiChoiceQuestionResult(bool IsSuccess);
}
