namespace Capstone.Application.QuestionDomain.Commands.ImportCsvTrueFalseQuestion
{
    public record ImportCsvTrueFalseQuestionCommand(Guid UserId, string Role, Guid ChapterId, IFormFile CsvFile) : ICommand<ImportCsvTrueFalseQuestionResult>;
    public record ImportCsvTrueFalseQuestionResult(bool IsSuccess);
    public class ImportCsvTrueFalseQuestionDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public bool IsTrueAnswer { get; set; } = default!;
        public int Difficulty { get; set; } = default!;
    };
}
