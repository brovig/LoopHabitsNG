using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
    }

    public DbSet<Habit>? Habits { get; set; }
    public DbSet<Repetition>? Repetitions { get; set; }
}
