using System.Reflection;
using Capstone.Application.Interface;
using Capstone.Domain.Identity.Models;
using Capstone.Domain.RescueTeam.Models;
using Capstone.Domain.UserAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Capstone.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<User> AppUsers => Set<User>();
    public DbSet<Rescue> Rescues => Set<Rescue>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}