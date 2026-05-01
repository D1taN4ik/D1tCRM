using D1tCRM.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace D1tCRM.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();
    public DbSet<EmployeeProject> EmployeeProjects => Set<EmployeeProject>();
    public DbSet<EmployeeTask> EmployeeTasks => Set<EmployeeTask>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MiddleName)
                .HasMaxLength(50);

            entity.Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Salary)
                .HasColumnType("numeric(12,2)");

            entity.HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.StartDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(p => p.EndDate)
                .HasColumnType("timestamp without time zone");
        });

        builder.Entity<WorkTask>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(t => t.Description)
                .HasMaxLength(1000);

            entity.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(t => t.StartDate)
                .HasColumnType("timestamp without time zone");

            entity.Property(t => t.EndDate)
                .HasColumnType("timestamp without time zone");

            entity.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<EmployeeProject>(entity =>
        {
            entity.HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            entity.Property(ep => ep.RoleInProject)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId);

            entity.HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId);
        });

        builder.Entity<EmployeeTask>(entity =>
        {
            entity.HasKey(et => new { et.EmployeeId, et.WorkTaskId });

            entity.HasOne(et => et.Employee)
                .WithMany(e => e.EmployeeTasks)
                .HasForeignKey(et => et.EmployeeId);

            entity.HasOne(et => et.WorkTask)
                .WithMany(t => t.EmployeeTasks)
                .HasForeignKey(et => et.WorkTaskId);
        });
    }
}