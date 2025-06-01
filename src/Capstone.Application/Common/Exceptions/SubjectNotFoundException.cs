namespace Capstone.Application.Common.Exceptions
{
    public class SubjectNotFoundException(Guid Id) : NotFoundException($"Môn học với Id: {Id} không tìm thấy")
    {
    }
}
