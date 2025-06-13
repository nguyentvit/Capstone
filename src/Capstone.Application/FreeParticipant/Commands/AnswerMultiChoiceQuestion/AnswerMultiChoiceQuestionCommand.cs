namespace Capstone.Application.FreeParticipant.Commands.AnswerMultiChoiceQuestion
{
    public record AnswerMultiChoiceQuestionCommand(Guid ParticipantId, Guid QuestionId, List<Guid> Answer) : ICommand<AnswerMultiChoiceQuestionResult>;
    public record AnswerMultiChoiceQuestionResult(bool IsSuccess);
}
