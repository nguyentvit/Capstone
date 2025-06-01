namespace Capstone.Application.ChapterDomain.Commands.AddChapter
{
    public record AddChapterCommand(Guid UserId, string Role, Guid SubjectId, string Title) : ICommand<AddChapterResult>;
    public record AddChapterResult(Guid Id);
    public class AddChapterCommandValidator : AbstractValidator<AddChapterCommand>
    {
        public AddChapterCommandValidator()
        {

        }
    }
}
