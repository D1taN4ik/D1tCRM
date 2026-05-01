using D1tCRM.Data.Entities;

namespace D1tCRM.Services.Tasks;

public interface ITaskService
{
    Task<List<WorkTask>> GetAllAsync();
    Task<WorkTask?> GetByIdAsync(int id);
    Task<int> CreateAsync(WorkTask task);
    Task UpdateAsync(WorkTask task);
    Task DeleteAsync(int id);

    Task AssignEmployeesAsync(int taskId, List<int> employeeIds);
    Task<List<int>> GetAssignedEmployeeIdsAsync(int taskId);
}