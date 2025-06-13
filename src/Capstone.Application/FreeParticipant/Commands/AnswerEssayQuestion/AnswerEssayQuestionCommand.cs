namespace Capstone.Application.FreeParticipant.Commands.AnswerEssayQuestion
{
    public record AnswerEssayQuestionCommand(Guid ParticipantId, Guid QuestionId, string Answer) : ICommand<AnswerEssayQuestionResult>;
    public record AnswerEssayQuestionResult(bool IsSuccess);
}
