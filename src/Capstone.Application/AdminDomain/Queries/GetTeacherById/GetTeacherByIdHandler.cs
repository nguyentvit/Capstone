namespace Capstone.Application.AdminDomain.Queries.GetTeacherById
{
    public class GetTeacherByIdHandler(IApplicationDbContext dbContext) : IQueryHandler<GetTeacherByIdQuery, GetTeacherByIdResult>
    {
        public async Task<GetTeacherByIdResult> Handle(GetTeacherByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(query.Id);
            var teacher = await dbContext.Teachers
                                        .AsNoTracking()
                                        .Where(t => t.Id == userId)
                                        .Select(t => new { t.Id, t.TeacherId, t.UserName, t.Phone, t.Avatar, t.IsActive, t.Email})
                                        .FirstOrDefaultAsync(cancellationToken);
            if (teacher == null)
                throw new TeacherNotFoundException(userId.Value);

            return new GetTeacherByIdResult(
                Id: teacher.Id.Value,
                TeacherId: teacher.TeacherId.Value,
                UserName: teacher.UserName.Value,
                Email: teacher.Email?.Value,
                PhoneNumber: teacher.Phone?.Value,
                Avartar: teacher.Avatar?.Url,
                IsActive: teacher.IsActive.Value
                );
            
        }
    }
}
