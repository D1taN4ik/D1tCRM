namespace D1tCRM.Data.Entities;

public class WorkTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = "New";

    public int ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public ICollection<EmployeeTask> EmployeeTasks { get; set; } = new List<EmployeeTask>();
}