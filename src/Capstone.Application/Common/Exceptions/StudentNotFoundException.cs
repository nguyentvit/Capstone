namespace Capstone.Application.Common.Exceptions
{
    public class StudentNotFoundException(Guid Id) : NotFoundException($"Sinh viên với Id: {Id} không tìm thấy")
    {
    }
}
