namespace Contracts;

public interface IRepositoryManager
{
    IHabitRepository HabitRepository { get; }
    IRepetitionRepository RepetitionRepository { get; }
    void Save();
}
