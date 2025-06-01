namespace Capstone.Application.StudentDomain.Commands.AnswerTrueFalseQuestion
{
    public record AnswerTrueFalseQuestionCommand(Guid UserId, Guid ExamSessionId, Guid QuestionId, bool Answer) : ICommand<AnswerTrueFalseQuestionResult>;
    public record AnswerTrueFalseQuestionResult(bool IsSuccess);
}
