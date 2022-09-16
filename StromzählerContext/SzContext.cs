
using Microsoft.EntityFrameworkCore;


namespace StromzählerContext;

public class SzContext : DbContext
{
    public DbSet<Counter> Counters { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<CounterValue> CounterValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server = (localDb)\\MSSQLLocalDb; Database =StromzählerEfCore; Trusted_Connection = True; ");
    }
}