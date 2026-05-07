namespace D1tCRM.Data.Entities;

public class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = "Active";

    public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();

    public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
}