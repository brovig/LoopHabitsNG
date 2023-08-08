using Entities.Models;

namespace Contracts;

public interface IRepetitionRepository
{
    Task<IEnumerable<Repetition>> GetAllRepetitionsAsync(Guid habitId, bool trackChanges);
    Task<Repetition> GetRepetitionAsync(Guid habitId, int id, bool trackChanges);
    void CreateRepetition(Guid habitId, Repetition repetition);
    void DeleteRepetition(Repetition repetition);
}
