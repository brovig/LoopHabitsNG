using Entities.SeedDataModels;

namespace Contracts;

public interface ISqliteBackupRepository
{
    Task<IEnumerable<Habit>> GetAllHabitsFromBackupAsync();
    Task<IEnumerable<Repetition>> GetAllRepetitionsFromBackupAsync();
}
