using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class HabitRepository : RepositoryBase<Habit>, IHabitRepository
{
    public HabitRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }


    public async Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, bool trackChanges)
    {
        return await FindByCondition(h => h.UserId.Equals(userId), trackChanges)
            .OrderBy(h => h.Position)
            .ToListAsync();
    }

    public async Task<IEnumerable<Habit>> GetAllHabitsWithRepsAsync(string userId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        return await FindByCondition(h => h.UserId.Equals(userId), trackChanges)
            .OrderBy(h => h.Position)
            .Include(h => h.Repetitions.Where(r => r.Timestamp >= repetitionParameters.StartDate
                                                   && r.Timestamp <= repetitionParameters.EndDate))
            .ToListAsync();
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
