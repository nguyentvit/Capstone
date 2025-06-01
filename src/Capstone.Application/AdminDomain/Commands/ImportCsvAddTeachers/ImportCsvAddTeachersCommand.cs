namespace Capstone.Application.AdminDomain.Commands.ImportCsvAddTeachers
{
    public record ImportCsvAddTeachersCommand(IFormFile CsvFile) : ICommand<ImportCsvAddTeachersResult>;
    public record ImportCsvAddTeachersResult(List<ImportCsvAddTeachersDto> Teachers);
    public class ImportCsvAddTeachersCommandValidator : AbstractValidator<ImportCsvAddTeachersCommand>
    {
        public ImportCsvAddTeachersCommandValidator()
        {

        }
            
    }
    public class ImportCsvAddTeachersDto
    {
        public string UserName { get; set; } = default!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string TeacherId { get; set; } = default!;
        public Guid Id { get; set; } = default!;
    }
    public record ImportCsvAddTeachersError
    {

    };
}
