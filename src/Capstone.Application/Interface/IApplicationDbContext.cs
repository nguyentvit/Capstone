namespace Capstone.Application.Interface;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> ApplicationUsers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}