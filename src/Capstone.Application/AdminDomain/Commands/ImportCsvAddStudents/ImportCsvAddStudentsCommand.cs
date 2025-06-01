namespace Capstone.Application.AdminDomain.Commands.ImportCsvAddStudents
{
    public record ImportCsvAddStudentsCommand(IFormFile CsvFile) : ICommand<ImportCsvAddStudentsResult>;
    public record ImportCsvAddStudentsResult(List<ImportCsvAddStudentsDto> Students);
    public class ImportCsvAddStudentsCommandValidator : AbstractValidator<ImportCsvAddStudentsCommand>
    {
        public ImportCsvAddStudentsCommandValidator()
        {

        }
    }
    public class ImportCsvAddStudentsDto
    {
        public string UserName { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string StudentId { get; set; } = default!;
        public Guid Id { get; set; } = default!;
    }
    public class ImportCsvAddStudentsError
    {

    };
}
