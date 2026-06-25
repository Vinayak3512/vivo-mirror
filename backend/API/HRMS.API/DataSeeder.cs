using UserFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API
{
    public static class DataSeeder
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<PostgresDbContext>();

            // Ensure DB is created
            context.Database.EnsureCreated();

            if (!context.Set<Employee>().Any(e => e.Email == "admin@propvivo.com"))
            {
                var admin = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@propvivo.com",
                    Role = "Admin",
                    Status = "Active",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow
                };

                context.Set<Employee>().Add(admin);
                context.SaveChanges();
            }
        }
    }
}
