namespace Capstone.Application.Common.Exceptions
{
    public class TeacherDuplicateExpcetion(string TeacherId) : BadRequestException($"Giảng viên với MSGV: {TeacherId} đã tồn tại")
    {
    }
}
