using CsvHelper.Configuration;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvSingleChoiceQuestion
{
    public record ImportCsvSingleChoiceQuestionCommand(Guid UserId, string Role, Guid ChapterId, IFormFile CsvFile) : ICommand<ImportCsvSingleChoiceQuestionResult>;
    public record ImportCsvSingleChoiceQuestionResult(bool IsSuccess);
    public class ImportCsvSingleChoiceQuestionDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Difficulty { get; set; } = default!;
        public int CorrectAnswerIndex { get; set; } = default!;
        public List<string> Choices { get; set; } = default!;
    }
    public class ImportCsvSingleChoiceQuestionDtoMap : ClassMap<ImportCsvSingleChoiceQuestionDto>
    {
        public ImportCsvSingleChoiceQuestionDtoMap()
        {
            Map(m => m.Title);
            Map(m => m.Content);
            Map(m => m.Difficulty);
            Map(m => m.CorrectAnswerIndex);
            Map(m => m.Choices).Convert(row =>
            {
                var raw = row.Row.GetField("Choices")!;
                return raw.Split("||", StringSplitOptions.RemoveEmptyEntries)
                          .Select(s => s.Trim())
                          .ToList();
            });
        }
    }
}
