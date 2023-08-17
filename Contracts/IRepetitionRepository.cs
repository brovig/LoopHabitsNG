using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IRepetitionRepository
{
    Task<IEnumerable<Repetition>> GetRepetitionsAsync(Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges);
    Task<Repetition> GetRepetitionAsync(Guid habitId, int id, bool trackChanges);
    void CreateRepetition(Guid habitId, Repetition repetition);
    void DeleteRepetition(Repetition repetition);
}