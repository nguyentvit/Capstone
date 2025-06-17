namespace Capstone.Application.QuestionDomain.Commands.ImportCsvEssayQuestion
{
    public record ImportCsvEssayQuestionCommand(Guid UserId, string Role, Guid ChapterId, IFormFile CsvFile) : ICommand<ImportCsvEssayQuestionResult>;
    public record ImportCsvEssayQuestionResult(bool IsSuccess);
    public class ImportCsvEssayQuestionDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Difficulty { get; set; } = default!;
    }
}
