namespace Capstone.Application.FreeParticipant.Commands.AnswerTrueFalseQuestion
{
    public record AnswerTrueFalseQuestionCommand(Guid ParticipantId, Guid QuestionId, bool Answer) : ICommand<AnswerTrueFalseQuestionResult>;
    public record AnswerTrueFalseQuestionResult(bool IsSuccess);
}
