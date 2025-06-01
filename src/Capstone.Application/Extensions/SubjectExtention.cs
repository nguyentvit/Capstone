using Capstone.Domain.SubjectDomain.Models;

namespace Capstone.Application.Extensions
{
    public static class SubjectExtention
    {
        public static void CheckRole(Subject subject, UserId userId, string role)
        {
            if (subject is SystemSubject)
            {
                if (role != RoleConstant.ADMIN)
                    throw new AuthenticationException("Chỉ có admin mới thêm được câu hỏi vào ngân hàng câu hỏi");
            }
            else if (subject is TeacherSubject teacherSubject)
            {
                if (teacherSubject.OwnerId != userId)
                    throw new AuthenticationException("bạn không có quyền thêm câu hỏi vào môn học của người khác");
            }
            else
            {
                throw new InvalidOperationException("Môn học này không phải là ngân hàng câu hỏi hoặc môn học của giảng viên");
            }
        }
    }
}
