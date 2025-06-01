namespace Capstone.Application.QuestionDomain.Commands.UpdateSingleChoiceQuestion
{
    public record UpdateSingleChoiceQuestionCommand(Guid UserId, Guid QuestionId, string Title, string Content, int Difficulty, List<string> Choices, int CorrectAnswerIndex) : ICommand<UpdateSingleChoiceQuestionResult>;
    public record UpdateSingleChoiceQuestionResult(Guid Id);
    public class UpdateSingleChoiceQuestionCommandValidator : AbstractValidator<UpdateSingleChoiceQuestionCommand>
    {
        public UpdateSingleChoiceQuestionCommandValidator()
        {

        }
    }
}
