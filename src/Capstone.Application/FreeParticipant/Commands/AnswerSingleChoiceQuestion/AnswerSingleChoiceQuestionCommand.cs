namespace Capstone.Application.FreeParticipant.Commands.AnswerSingleChoiceQuestion
{
    public record AnswerSingleChoiceQuestionCommand(Guid ParticipantId, Guid QuestionId, Guid Answer) : ICommand<AnswerSingleChoiceQuestionResult>;
    public record AnswerSingleChoiceQuestionResult(bool IsSuccess);
}
