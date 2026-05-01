using D1tCRM.Data.Entities;

namespace D1tCRM.Services.Projects;

public interface IProjectService
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<int> CreateAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(int id);
    Task AssignEmployeesAsync(int projectId, List<int> employeeIds);
    Task<List<int>> GetAssignedEmployeeIdsAsync(int projectId);
    Task<List<Project>> GetByEmployeeIdAsync(int employeeId);
}