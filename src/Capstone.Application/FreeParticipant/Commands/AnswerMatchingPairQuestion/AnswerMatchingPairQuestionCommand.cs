namespace Capstone.Application.FreeParticipant.Commands.AnswerMatchingPairQuestion
{
    public record AnswerMatchingPairQuestionCommand(Guid ParticipantId, Guid QuestionId, Dictionary<Guid, Guid> Answer) : ICommand<AnswerMatchingPairQuestionResult>;
    public record AnswerMatchingPairQuestionResult(bool IsSuccess);
}
