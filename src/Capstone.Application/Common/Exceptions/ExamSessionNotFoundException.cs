namespace Capstone.Application.Common.Exceptions
{
    public class ExamSessionNotFoundException(Guid Id) : NotFoundException($"Phiên thi với Id: {Id} không tồn tại")
    {
    }
}
