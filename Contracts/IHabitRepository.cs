using Entities.Models;

namespace Contracts;

public interface IHabitRepository
{
    Task<IEnumerable<Habit>> GetAllHabitsAsync(bool trackChanges);
    Task<Habit> GetHabitAsync(Guid habitId, bool trackChanges);
    Task<IEnumerable<Habit>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void CreateHabit(Habit habit);
    void DeleteHabit(Habit habit);
}
