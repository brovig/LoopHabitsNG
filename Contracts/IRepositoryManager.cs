namespace Contracts;

public interface IRepositoryManager
{
    IHabitRepository HabitRepository { get; }
    IRepetitionRepository RepetitionRepository { get; }
    ISqliteBackupRepository SqliteBackupRepository { get; }
    void Save();
}
