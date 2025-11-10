using Microsoft.EntityFrameworkCore;

namespace UserManagement.Data.Tests;

public class TestDbContext<T> : DbContext
    where T : class
{
    public DbSet<T> Models { get; set; } = null!;

    public TestDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<T>();
    }
}
