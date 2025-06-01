namespace Capstone.Application.QuestionDomain.Commands.UpdateTrueFalseQuestion
{
    public record UpdateTrueFalseQuestionCommand(Guid UserId, Guid QuestionId, string Title, string Content, bool IsTrueAnswer, int Difficulty) : ICommand<UpdateTrueFalseQuestionResult>;
    public record UpdateTrueFalseQuestionResult(Guid Id);
}
