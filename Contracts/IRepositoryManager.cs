namespace Contracts;

public interface IRepositoryManager
{
    IHabitRepository Habit { get; }
    IRepetitionRepository Repetition { get; }
    ISqliteBackupRepository SqliteBackup { get; }
    Task SaveAsync();
}
