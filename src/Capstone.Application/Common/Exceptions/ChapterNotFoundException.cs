namespace Capstone.Application.Common.Exceptions
{
    public class ChapterNotFoundException(Guid Id) : NotFoundException($"Chương với Id: {Id} không tồn tại")
    {
    }
}
