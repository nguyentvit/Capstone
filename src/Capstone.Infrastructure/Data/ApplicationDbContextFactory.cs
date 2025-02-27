// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;

// namespace Capstone.Infrastructure.Data;
// public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
//     {
//         public ApplicationDbContext CreateDbContext(string[] args)
//         {
//              var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

//         var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Capstone.API"); // Đường dẫn đến API
//         var configuration = new ConfigurationBuilder()
//             .SetBasePath(basePath)  // Sử dụng thư mục của API làm gốc
//             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//             .Build();

//         var connectionString = configuration.GetConnectionString("Database");
//         optionsBuilder.UseSqlServer(connectionString);

//         return new ApplicationDbContext(optionsBuilder.Options);
//         }
//     }