namespace Capstone.Application.QuestionDomain.Commands.UpdateMultiChoiceQuestion
{
    public record UpdateMultiChoiceQuestionCommand(Guid UserId, Guid QuestionId, string Title, string Content, int Difficulty, Dictionary<string, bool> Choices) : ICommand<UpdateMultiChoiceQuestionResult>;
    public record UpdateMultiChoiceQuestionResult(Guid Id);
    public class UpdateMultiChoiceQuestionCommandValidator : AbstractValidator<UpdateMultiChoiceQuestionCommand>
    {
        public UpdateMultiChoiceQuestionCommandValidator()
        {

        }
    }
}
