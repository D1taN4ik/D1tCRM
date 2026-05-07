namespace D1tCRM.Data.Entities;

public class EmployeeTask
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;

    public int WorkTaskId { get; set; }

    public WorkTask WorkTask { get; set; } = null!;
}