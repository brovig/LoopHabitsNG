using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class HabitRepository : RepositoryBase<Habit>, IHabitRepository
{
    public HabitRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }


    public async Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, bool trackChanges)
    {
        return await FindByCondition(h => h.UserId.Equals(userId), trackChanges).OrderBy(p => p.Position).ToListAsync();
    }

    public async Task<IEnumerable<Habit>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        return await FindByCondition(x => ids.Contains(x.Id), trackChanges).ToListAsync();
    }

    public async Task<Habit> GetHabitAsync(Guid habitId, bool trackChanges)
    {
        return await FindByCondition(h => h.Id.Equals(habitId), trackChanges).SingleOrDefaultAsync();
    }

    public void CreateHabit(Habit habit)
    {
        Create(habit);
    }

    public void DeleteHabit(Habit habit)
    {
        Delete(habit);
    }
}
