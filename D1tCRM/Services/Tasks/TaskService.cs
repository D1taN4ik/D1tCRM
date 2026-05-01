using D1tCRM.Data;
using D1tCRM.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace D1tCRM.Services.Tasks;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<WorkTask>> GetAllAsync()
    {
        return await _db.WorkTasks
            .Include(t => t.Project)
            .Include(t => t.EmployeeTasks)
                .ThenInclude(et => et.Employee)
            .OrderByDescending(t => t.StartDate)
            .ToListAsync();
    }

    public async Task<WorkTask?> GetByIdAsync(int id)
    {
        return await _db.WorkTasks
            .Include(t => t.Project)
            .Include(t => t.EmployeeTasks)
                .ThenInclude(et => et.Employee)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<int> CreateAsync(WorkTask task)
    {
        _db.WorkTasks.Add(task);
        await _db.SaveChangesAsync();

        return task.Id;
    }

    public async Task UpdateAsync(WorkTask task)
    {
        var existingTask = await _db.WorkTasks
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        if (existingTask is null)
            return;

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.ProjectId = task.ProjectId;
        existingTask.StartDate = task.StartDate;
        existingTask.EndDate = task.EndDate;
        existingTask.Status = task.Status;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var task = await _db.WorkTasks.FindAsync(id);

        if (task is null)
            return;

        _db.WorkTasks.Remove(task);
        await _db.SaveChangesAsync();
    }

    public async Task AssignEmployeesAsync(int taskId, List<int> employeeIds)
    {
        var oldLinks = await _db.EmployeeTasks
            .Where(et => et.WorkTaskId == taskId)
            .ToListAsync();

        _db.EmployeeTasks.RemoveRange(oldLinks);

        var newLinks = employeeIds.Select(employeeId => new EmployeeTask
        {
            WorkTaskId = taskId,
            EmployeeId = employeeId
        });

        await _db.EmployeeTasks.AddRangeAsync(newLinks);
        await _db.SaveChangesAsync();
    }

    public async Task<List<int>> GetAssignedEmployeeIdsAsync(int taskId)
    {
        return await _db.EmployeeTasks
            .Where(et => et.WorkTaskId == taskId)
            .Select(et => et.EmployeeId)
            .ToListAsync();
    }
}