namespace Capstone.Application.ExamSessionModule.Commands.MarkEssayQuestion
{
    public record MarkEssayQuestionCommand(Guid UserId, Guid ParticipantId, Guid QuestionId, double Score) : ICommand<MarkEssayQuestionResult>;
    public record MarkEssayQuestionResult(bool IsSuccess);
}
