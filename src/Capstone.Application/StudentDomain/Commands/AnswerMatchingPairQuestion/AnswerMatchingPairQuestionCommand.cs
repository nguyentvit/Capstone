namespace Capstone.Application.StudentDomain.Commands.AnswerMatchingPairQuestion
{
    public record AnswerMatchingPairQuestionCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId, Dictionary<Guid, Guid> Answer) : ICommand<AnswerMatchingPairQuestionResult>;
    public record AnswerMatchingPairQuestionResult(bool IsSuccess);
}
