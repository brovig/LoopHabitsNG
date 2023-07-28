using Contracts;
using Entities.Models;

namespace Repository;

public class HabitRepository : RepositoryBase<Habit>, IHabitRepository
{
    public HabitRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
