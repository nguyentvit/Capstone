namespace Capstone.Application.AdminDomain.Queries.GetStudentById
{
    public class GetStudentByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetStudentByIdQuery, GetStudentByIdResult>
    {
        public async Task<GetStudentByIdResult> Handle(GetStudentByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.Id);
            var student = await dbContext.Students
                                        .AsNoTracking()
                                        .Where(s => s.Id == userId)
                                        .Select(s => new { s.Id, s.StudentId, s.UserName, s.Phone, s.Avatar, s.IsActive, s.Email })
                                        .FirstOrDefaultAsync(cancellationToken);

            if (student == null)
                throw new StudentNotFoundException(userId.Value);

            return new GetStudentByIdResult(
                Id: student.Id.Value,
                StudentId: student.StudentId.Value,
                UserName: student.UserName.Value,
                Email: student.Email?.Value,
                PhoneNumber: student.Phone?.Value,
                Avatar: student.Avatar?.Url,
                IsActive: student.IsActive.Value
                );
        }
    }
}
