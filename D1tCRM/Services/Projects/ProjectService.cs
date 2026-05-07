using D1tCRM.Data;
using D1tCRM.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace D1tCRM.Services.Projects;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _db;

    public ProjectService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _db.Projects
            .Include(p => p.Tasks)
            .Include(p => p.EmployeeProjects)
                .ThenInclude(ep => ep.Employee)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _db.Projects
            .Include(p => p.Tasks)
            .Include(p => p.EmployeeProjects)
                .ThenInclude(ep => ep.Employee)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<int> CreateAsync(Project project)
    {
        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return project.Id;
    }

    public async Task UpdateAsync(Project project)
    {
        var existingProject = await _db.Projects
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        if (existingProject is null)
            return;

        existingProject.Name = project.Name;
        existingProject.Description = project.Description;
        existingProject.StartDate = project.StartDate;
        existingProject.EndDate = project.EndDate;
        existingProject.Status = project.Status;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var project = await _db.Projects.FindAsync(id);

        if (project is null)
            return;

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();
    }

    public async Task AssignEmployeesAsync(int projectId, List<int> employeeIds)
    {
        var oldLinks = await _db.EmployeeProjects
            .Where(ep => ep.ProjectId == projectId)
            .ToListAsync();

        _db.EmployeeProjects.RemoveRange(oldLinks);

        var newLinks = employeeIds.Select(employeeId => new EmployeeProject
        {
            ProjectId = projectId,
            EmployeeId = employeeId,
            RoleInProject = "Member"
        });

        await _db.EmployeeProjects.AddRangeAsync(newLinks);
        await _db.SaveChangesAsync();
    }

    public async Task<List<int>> GetAssignedEmployeeIdsAsync(int projectId)
    {
        return await _db.EmployeeProjects
            .Where(ep => ep.ProjectId == projectId)
            .Select(ep => ep.EmployeeId)
            .ToListAsync();
    }
    public async Task<List<Project>> GetByEmployeeIdAsync(int employeeId)
    {
        return await _db.EmployeeProjects
            .Where(ep => ep.EmployeeId == employeeId)
            .Include(ep => ep.Project)
                .ThenInclude(p => p.Tasks)
            .Include(ep => ep.Project)
                .ThenInclude(p => p.EmployeeProjects)
                    .ThenInclude(x => x.Employee)
            .Select(ep => ep.Project)
            .ToListAsync();
    }
}