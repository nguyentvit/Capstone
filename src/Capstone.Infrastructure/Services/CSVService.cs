using Capstone.Application.Interface.Services;
using Capstone.Application.QuestionDomain.Commands.ImportCsvMatchingQuestion;
using Capstone.Application.QuestionDomain.Commands.ImportCsvMultiChoiceQuestion;
using Capstone.Application.QuestionDomain.Commands.ImportCsvSingleChoiceQuestion;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace Capstone.Infrastructure.Services
{
    public class CSVService : ICSVService
    {
        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            using var reader = new StreamReader(file);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                BadDataFound = args => Console.WriteLine($"Bad data found: {args.RawRecord}")
            };

            using var csv = new CsvReader(reader, config);

            if (typeof(T) == typeof(ImportCsvSingleChoiceQuestionDto))
            {
                csv.Context.RegisterClassMap<ImportCsvSingleChoiceQuestionDtoMap>();
            }
            else if (typeof(T) == typeof(ImportCsvMultiChoiceQuestionDto))
            {
                csv.Context.RegisterClassMap<ImportCsvMultiChoiceQuestionDtoMap>();
            }
            else if (typeof(T) == typeof(ImportCsvMatchingQuestionDto))
            {
                csv.Context.RegisterClassMap<ImportCsvMatchingQuestionDtoMap>();
            }

                return csv.GetRecords<T>().ToList();
        }

        public void WriteCSV<T>(List<T> records)
        {
            using (var writer = new StreamWriter("D:\\file.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
