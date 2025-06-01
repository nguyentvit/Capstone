namespace Capstone.Application.Common.Exceptions
{
    public class TeacherNotFoundException(Guid Id) : NotFoundException($"Giảng viên với Id: {Id} không tìm thấy")
    {
    }
}
