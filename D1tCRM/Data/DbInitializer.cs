using D1tCRM.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace D1tCRM.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await db.Database.MigrateAsync();

        string[] roles = ["Admin", "Manager", "Employee"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        const string adminEmail = "admin@d1tcrm.local";
        const string adminPassword = "Admin12345!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser is not null)
        {
            return;
        }

        var adminEmployee = new Employee
        {
            LastName = "Администратор",
            FirstName = "D1tCRM",
            MiddleName = null,
            Position = "Системный администратор",
            Salary = 0,
            IsActive = true
        };

        db.Employees.Add(adminEmployee);
        await db.SaveChangesAsync();

        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            EmployeeId = adminEmployee.Id
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (!result.Succeeded)
        {
            throw new Exception(
                "Не удалось создать администратора: " +
                string.Join("; ", result.Errors.Select(e => e.Description))
            );
        }

        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}