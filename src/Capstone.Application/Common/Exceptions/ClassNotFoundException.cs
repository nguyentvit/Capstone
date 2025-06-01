namespace Capstone.Application.Common.Exceptions
{
    public class ClassNotFoundException(Guid Id) : NotFoundException($"Lớp học với Id: {Id} không tồn tại")
    {
    }
}
