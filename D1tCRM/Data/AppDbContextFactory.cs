using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace D1tCRM.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=d1tcrm;Username=d1tcrm_user;Password=d1tcrm_password"
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}