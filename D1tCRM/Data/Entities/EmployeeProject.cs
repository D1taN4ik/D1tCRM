namespace D1tCRM.Data.Entities;

public class EmployeeProject
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public int ProjectId { get; set; }

    public Project Project { get; set; } = null!;

    public string RoleInProject { get; set; } = "Member";
}