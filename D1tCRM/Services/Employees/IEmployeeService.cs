using D1tCRM.Data.Entities;

namespace D1tCRM.Services.Employees;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task<Employee?> GetByIdAsync(int id);

    Task CreateWithUserAsync(
        string lastName,
        string firstName,
        string? middleName,
        string position,
        decimal salary,
        string email,
        string password,
        string role
    );
    Task UpdateWithUserAsync(
    int employeeId,
    string lastName,
    string firstName,
    string? middleName,
    string position,
    decimal salary,
    string email,
    string? newPassword,
    string role
    );
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
}