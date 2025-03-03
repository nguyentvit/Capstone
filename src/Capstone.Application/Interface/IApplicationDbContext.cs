using Capstone.Domain.RescueTeam.Models;
using Capstone.Domain.UserAccess.Models;

namespace Capstone.Application.Interface;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> ApplicationUsers { get; }
    DbSet<User> AppUsers { get; }
    DbSet<Rescue> Rescues { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}