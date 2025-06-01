namespace Capstone.Application.Common.Exceptions
{
    public class ExamVersionNotFoundException(Guid Id) : NotFoundException($"Đề thi với Id: {Id} không tồn tại")
    {
    }
}
