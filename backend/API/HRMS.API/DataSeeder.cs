using UserFeature.Domain;
using LeaveFeature.Domain;
using AttendanceFeature.Domain;
using DocumentsFeature.Domain;
using PayrollFeature.Domain;
using ExpensesFeature.Domain;
using AnnouncementsFeature.Domain;
using TodoFeature.Domain;
using HRMS.Core.Postgres.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

            // 1. Seed Employees
            if (!context.Set<Employee>().Any())
            {
                var admin = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = "EMP-001",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@propvivo.com",
                    Phone = "+1234567890",
                    Designation = "Administrator",
                    Department = "IT",
                    DateOfJoining = DateTime.UtcNow.AddYears(-2),
                    Role = "Admin",
                    Status = "Active",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Country = "US"
                };
                context.Set<Employee>().Add(admin);

                var emp1 = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = "EMP-002",
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice.johnson@company.com",
                    Phone = "+1234567891",
                    Designation = "Software Engineer",
                    Department = "Engineering",
                    DateOfJoining = DateTime.UtcNow.AddYears(-1),
                    Role = "Employee",
                    Status = "Active",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Country = "US"
                };
                context.Set<Employee>().Add(emp1);

                var emp2 = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = "EMP-003",
                    FirstName = "Bob",
                    LastName = "Smith",
                    Email = "bob.smith@company.com",
                    Phone = "+1234567892",
                    Designation = "Product Manager",
                    Department = "Product",
                    DateOfJoining = DateTime.UtcNow.AddMonths(-6),
                    Role = "Manager",
                    Status = "Active",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Country = "US"
                };
                context.Set<Employee>().Add(emp2);

                var emp3 = new Employee
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = "EMP-004",
                    FirstName = "Carol",
                    LastName = "Davis",
                    Email = "carol.davis@company.com",
                    Phone = "+1234567893",
                    Designation = "UI/UX Designer",
                    Department = "Design",
                    DateOfJoining = DateTime.UtcNow.AddMonths(-3),
                    Role = "Employee",
                    Status = "Active",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Country = "US"
                };
                context.Set<Employee>().Add(emp3);

                context.SaveChanges();
            }

            // Get employee IDs to link properly
            var employees = context.Set<Employee>().ToList();
            var alice = employees.FirstOrDefault(e => e.FirstName == "Alice");
            var bob = employees.FirstOrDefault(e => e.FirstName == "Bob");

            // 2. Seed Announcements
            if (!context.Set<Announcement>().Any())
            {
                context.Set<Announcement>().AddRange(
                    new Announcement
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "New Leave Policy Effective July 1st",
                        Content = "Please review the updated leave policy in the HR portal. The standard annual paid leave is increased from 15 to 18 days.",
                        Category = "Policy",
                        Priority = "High",
                        Scope = "Global",
                        PostedDate = DateTime.UtcNow.AddDays(-2),
                        ExpiryDate = DateTime.UtcNow.AddDays(30),
                        IsActive = true
                    },
                    new Announcement
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Company Outing - Beach Day!",
                        Content = "Join us for a fun day at the beach on Saturday, July 12th! Food, games, and transportation will be provided.",
                        Category = "Event",
                        Priority = "Medium",
                        Scope = "Global",
                        PostedDate = DateTime.UtcNow.AddDays(-1),
                        ExpiryDate = DateTime.UtcNow.AddDays(15),
                        IsActive = true
                    },
                    new Announcement
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Office Renovation Complete",
                        Content = "Our office renovation is now complete. Welcome to the new modern collaboration spaces on the 3rd floor!",
                        Category = "General",
                        Priority = "Low",
                        Scope = "Global",
                        PostedDate = DateTime.UtcNow.AddDays(-5),
                        ExpiryDate = DateTime.UtcNow.AddDays(10),
                        IsActive = true
                    }
                );
                context.SaveChanges();
            }

            // 3. Seed Leaves
            if (!context.Set<LeaveRequest>().Any() && alice != null)
            {
                context.Set<LeaveRequest>().AddRange(
                    new LeaveRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = alice.Id,
                        LeaveType = "Casual",
                        StartDate = DateTime.UtcNow.AddDays(10),
                        EndDate = DateTime.UtcNow.AddDays(12),
                        TotalDays = 3,
                        Reason = "Family function",
                        Status = "Approved",
                        ApprovedBy = "Admin",
                        ApprovedDate = DateTime.UtcNow
                    },
                    new LeaveRequest
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = alice.Id,
                        LeaveType = "Sick",
                        StartDate = DateTime.UtcNow.AddDays(-1),
                        EndDate = DateTime.UtcNow.AddDays(-1),
                        TotalDays = 1,
                        Reason = "Fever",
                        Status = "Approved",
                        ApprovedBy = "Admin",
                        ApprovedDate = DateTime.UtcNow
                    }
                );
                context.SaveChanges();
            }

            // 4. Seed Attendance
            if (!context.Set<AttendanceRecord>().Any() && alice != null && bob != null)
            {
                var today = DateTime.UtcNow.Date;
                context.Set<AttendanceRecord>().AddRange(
                    new AttendanceRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = alice.Id,
                        Date = today,
                        ClockInTime = today.AddHours(9),
                        ClockOutTime = today.AddHours(17),
                        Status = "Present",
                        TotalHours = 8
                    },
                    new AttendanceRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = bob.Id,
                        Date = today,
                        ClockInTime = today.AddHours(9).AddMinutes(45),
                        ClockOutTime = today.AddHours(18),
                        Status = "Late",
                        TotalHours = 8.25
                    },
                    new AttendanceRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        EmployeeId = alice.Id,
                        Date = today.AddDays(-1),
                        ClockInTime = today.AddDays(-1).AddHours(9),
                        ClockOutTime = today.AddDays(-1).AddHours(17),
                        Status = "Present",
                        TotalHours = 8
                    }
                );
                context.SaveChanges();
            }

            // 5. Seed Todos
            if (!context.Set<Todo>().Any())
            {
                context.Set<Todo>().AddRange(
                    new Todo
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Verify Q2 payroll records",
                        Description = "Make sure all salary components, PF deductions, and professional taxes are verified.",
                        DueDate = DateTime.UtcNow.AddDays(5),
                        IsCompleted = false
                    },
                    new Todo
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Approve pending leave requests",
                        Description = "Review and process leave applications submitted by team members.",
                        DueDate = DateTime.UtcNow.AddDays(2),
                        IsCompleted = false
                    },
                    new Todo
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = "Onboard Carol Davis",
                        Description = "Complete onboarding tasks and day-1 setup for Carol Davis.",
                        DueDate = DateTime.UtcNow.AddDays(1),
                        IsCompleted = true
                    }
                );
                context.SaveChanges();
            }

            // 6. Seed Payroll
            if (!context.Set<PayrollRecord>().Any() && alice != null && bob != null)
            {
                context.Set<PayrollRecord>().AddRange(
                    new PayrollRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = alice.Id,
                        PayPeriodStart = DateTime.UtcNow.AddMonths(-1).Date,
                        PayPeriodEnd = DateTime.UtcNow.Date,
                        GrossPay = 8000,
                        TotalDeductions = 1500,
                        NetPay = 6500,
                        Currency = "USD",
                        CountryCode = "US",
                        Status = "Paid",
                        BasicSalary = 5000,
                        HRA = 1500,
                        SpecialAllowance = 1000,
                        PF = 600,
                        IncomeTax = 800,
                        HealthInsurance = 100
                    },
                    new PayrollRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = bob.Id,
                        PayPeriodStart = DateTime.UtcNow.AddMonths(-1).Date,
                        PayPeriodEnd = DateTime.UtcNow.Date,
                        GrossPay = 10000,
                        TotalDeductions = 2000,
                        NetPay = 8000,
                        Currency = "USD",
                        CountryCode = "US",
                        Status = "Paid",
                        BasicSalary = 6000,
                        HRA = 2000,
                        SpecialAllowance = 1500,
                        Bonus = 500,
                        PF = 720,
                        IncomeTax = 1100,
                        HealthInsurance = 180
                    }
                );
                context.SaveChanges();
            }

            // 7. Seed Expenses
            if (!context.Set<ExpenseRecord>().Any() && alice != null)
            {
                context.Set<ExpenseRecord>().AddRange(
                    new ExpenseRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = alice.Id,
                        ExpenseDate = DateTime.UtcNow.AddDays(-10),
                        Category = "Travel",
                        Amount = 150,
                        Currency = "USD",
                        Description = "Flight to client meeting in New York",
                        Status = "Approved",
                        ApprovedByUserId = "Admin"
                    },
                    new ExpenseRecord
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = alice.Id,
                        ExpenseDate = DateTime.UtcNow.AddDays(-3),
                        Category = "Food",
                        Amount = 45.5m,
                        Currency = "USD",
                        Description = "Dinner with client",
                        Status = "Pending-Approval"
                    }
                );
                context.SaveChanges();
            }

            // 8. Seed Documents
            if (!context.Set<EmployeeDocument>().Any() && alice != null)
            {
                context.Set<EmployeeDocument>().AddRange(
                    new EmployeeDocument
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = alice.Id,
                        FileName = "Passport_Alice.pdf",
                        FileUrl = "/documents/passport_alice.pdf",
                        Category = "identity",
                        Status = "verified",
                        ExpiryDate = DateTime.UtcNow.AddYears(5)
                    },
                    new EmployeeDocument
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = alice.Id,
                        FileName = "Employment_Agreement_Alice.pdf",
                        FileUrl = "/documents/employment_agreement_alice.pdf",
                        Category = "employment",
                        Status = "verified"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
