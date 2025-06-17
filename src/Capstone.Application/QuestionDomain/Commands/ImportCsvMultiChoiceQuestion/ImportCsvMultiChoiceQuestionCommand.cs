using CsvHelper.Configuration;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvMultiChoiceQuestion
{
    public record ImportCsvMultiChoiceQuestionCommand(Guid UserId, string Role, Guid ChapterId, IFormFile Csv) : ICommand<ImportCsvMultiChoiceQuestionResult>;
    public record ImportCsvMultiChoiceQuestionResult(bool IsSuccess);
    public class ImportCsvMultiChoiceQuestionDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int Difficulty { get; set; } = default!;
        public Dictionary<string, bool> Choices { get; set; } = default!;
    }
    public class ImportCsvMultiChoiceQuestionDtoMap : ClassMap<ImportCsvMultiChoiceQuestionDto>
    {
        public ImportCsvMultiChoiceQuestionDtoMap()
        {
            Map(m => m.Title);
            Map(m => m.Content);
            Map(m => m.Difficulty);

            Map(m => m.Choices).Convert(row =>
            {
                var raw = row.Row.GetField("Choices");
                return raw!.Split("||", StringSplitOptions.RemoveEmptyEntries)
                          .Select(choice =>
                          {
                              var parts = choice.Split(':', 2);
                              if (parts.Length != 2)
                                  throw new FormatException($"Invalid choice format: {choice}");

                              var key = parts[0].Trim();
                              var value = bool.Parse(parts[1].Trim());
                              return new KeyValuePair<string, bool>(key, value);
                          })
                          .ToDictionary(kv => kv.Key, kv => kv.Value);
            });
        }
    }
}
