using CsvHelper.Configuration;

namespace Capstone.Application.QuestionDomain.Commands.ImportCsvMatchingQuestion
{
    public record ImportCsvMatchingQuestionCommand(Guid UserId, string Role, Guid ChapterId, IFormFile Csv) : ICommand<ImportCsvMatchingQuestionResult>;
    public record ImportCsvMatchingQuestionResult(bool IsSuccess);
    public class ImportCsvMatchingQuestionDto
    {
        public string Title { get; set; } = default!;
        public string Content {  get; set; } = default!;
        public int Difficulty { get; set; } = default!;
        public Dictionary<string, string> MatchingPairs { get; set; } = default!;
    }
    public class ImportCsvMatchingQuestionDtoMap : ClassMap<ImportCsvMatchingQuestionDto>
    {
        public ImportCsvMatchingQuestionDtoMap()
        {
            Map(m => m.Title);
            Map(m => m.Content);
            Map(m => m.Difficulty);
            Map(m => m.MatchingPairs).Convert(row =>
            {
                var raw = row.Row.GetField("MatchingPairs");
                return raw!.Split("||", StringSplitOptions.RemoveEmptyEntries)
                          .Select(pair =>
                          {
                              var parts = pair.Split(':', 2);
                              if (parts.Length != 2)
                                  throw new FormatException($"Invalid matching pair format: {pair}");

                              var key = parts[0].Trim();
                              var value = parts[1].Trim();
                              return new KeyValuePair<string, string>(key, value);
                          })
                          .ToDictionary(kv => kv.Key, kv => kv.Value);
            });
        }

    }
}
