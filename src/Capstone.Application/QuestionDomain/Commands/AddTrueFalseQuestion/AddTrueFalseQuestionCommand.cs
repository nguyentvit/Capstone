namespace Capstone.Application.QuestionDomain.Commands.AddTrueFalseQuestion
{
    public record AddTrueFalseQuestionCommand(Guid UserId, Guid? ChapterId, string Title, string Content, bool IsTrueAnswer, int Difficulty, string Role) : ICommand<AddTrueFalseQuestionResult>;
    public record AddTrueFalseQuestionResult(Guid Id);
    public class AddTrueFalseQuestionCommandValidator : AbstractValidator<AddTrueFalseQuestionCommand>
    {
        public AddTrueFalseQuestionCommandValidator()
        {

        }
    }
}
