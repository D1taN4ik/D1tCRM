namespace D1tCRM.Data.Entities;

public class Employee
{
    public int Id { get; set; }

    public string LastName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public string Position { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public bool IsActive { get; set; } = true;

    public ApplicationUser? User { get; set; }

    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();

    public ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
}