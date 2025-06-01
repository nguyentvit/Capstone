namespace Capstone.Application.Extensions
{
    public static class CommonExtension
    {
        public static string GetSchoolIdFromUser(User user)
        {
            return user switch
            {
                Teacher teacher when teacher.TeacherId is not null => teacher.TeacherId.Value,
                Student student when student.StudentId is not null => student.StudentId.Value,
                _ => throw new InvalidOperationException("Người dùng không phải là giảng viên hoặc sinh viên")
            };
        }
    }
}
