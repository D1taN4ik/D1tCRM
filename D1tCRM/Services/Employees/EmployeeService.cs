using D1tCRM.Data;
using D1tCRM.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace D1tCRM.Services.Employees;

public class EmployeeService : IEmployeeService
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public EmployeeService(AppDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _db.Employees
            .Include(e => e.User)
            .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        return await _db.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task CreateAsync(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
    }

    public async Task CreateWithUserAsync(
        string lastName,
        string firstName,
        string? middleName,
        string position,
        decimal salary,
        string email,
        string password,
        string role)
    {
        var employee = new Employee
        {
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            Position = position,
            Salary = salary,
            IsActive = true
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            EmployeeId = employee.Id
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            _db.Employees.Remove(employee);
            await _db.SaveChangesAsync();

            throw new Exception(
                string.Join("; ", result.Errors.Select(e => e.Description))
            );
        }

        await _userManager.AddToRoleAsync(user, role);
    }

    public async Task UpdateAsync(Employee employee)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await _db.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (employee is null)
            return;

        if (employee.User is not null)
        {
            await _userManager.DeleteAsync(employee.User);
        }

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
    }
    public async Task UpdateWithUserAsync(
    int employeeId,
    string lastName,
    string firstName,
    string? middleName,
    string position,
    decimal salary,
    string email,
    string? newPassword,
    string role)
    {
        var employee = await _db.Employees
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

        if (employee is null)
            throw new Exception("Сотрудник не найден");

        employee.LastName = lastName;
        employee.FirstName = firstName;
        employee.MiddleName = middleName;
        employee.Position = position;
        employee.Salary = salary;

        if (employee.User is null)
            throw new Exception("Аккаунт сотрудника не найден");

        var user = employee.User;

        user.Email = email;
        user.UserName = email;
        user.NormalizedEmail = email.ToUpperInvariant();
        user.NormalizedUserName = email.ToUpperInvariant();
        user.EmailConfirmed = true;

        var updateUserResult = await _userManager.UpdateAsync(user);

        if (!updateUserResult.Succeeded)
        {
            throw new Exception(
                string.Join("; ", updateUserResult.Errors.Select(e => e.Description))
            );
        }

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var passwordResult = await _userManager.ResetPasswordAsync(
                user,
                token,
                newPassword
            );

            if (!passwordResult.Succeeded)
            {
                throw new Exception(
                    string.Join("; ", passwordResult.Errors.Select(e => e.Description))
                );
            }
        }

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeRolesResult.Succeeded)
            {
                throw new Exception(
                    string.Join("; ", removeRolesResult.Errors.Select(e => e.Description))
                );
            }
        }

        var addRoleResult = await _userManager.AddToRoleAsync(user, role);

        if (!addRoleResult.Succeeded)
        {
            throw new Exception(
                string.Join("; ", addRoleResult.Errors.Select(e => e.Description))
            );
        }

        await _db.SaveChangesAsync();
    }
}
