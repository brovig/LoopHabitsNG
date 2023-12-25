using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IHabitRepository
{
    Task<IEnumerable<Habit>> GetAllHabitsAsync(string userId, bool trackChanges);
    Task<IEnumerable<Habit>> GetAllHabitsWithRepsAsync(string userId, RepetitionParameters repetitionParameters, bool trackChanges);
    Task<Habit> GetHabitAsync(Guid habitId, bool trackChanges);
    Task<IEnumerable<Habit>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    void CreateHabit(Habit habit);
    void DeleteHabit(Habit habit);
}
