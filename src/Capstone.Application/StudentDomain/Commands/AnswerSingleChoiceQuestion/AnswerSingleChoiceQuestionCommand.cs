namespace Capstone.Application.StudentDomain.Commands.AnswerSingleChoiceQuestion
{
    public record AnswerSingleChoiceQuestionCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId, Guid Answer) : ICommand<AnswerSingleChoiceQuestionResult>;
    public record AnswerSingleChoiceQuestionResult(bool IsSuccess);
}
