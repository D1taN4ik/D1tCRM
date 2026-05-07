using Microsoft.AspNetCore.Identity;

namespace D1tCRM.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}