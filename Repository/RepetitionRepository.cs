using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class RepetitionRepository : RepositoryBase<Repetition>, IRepetitionRepository
{
    public RepetitionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Repetition>> GetRepetitionsAsync(Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        return await FindByCondition(r => r.HabitId.Equals(habitId) && r.Timestamp >= repetitionParameters.StartDate
                                    && r.Timestamp <= repetitionParameters.EndDate, trackChanges)
                                    .OrderByDescending(r => r.Timestamp)
                                    .ToListAsync();
    }

    public async Task<Repetition> GetRepetitionAsync(Guid habitId, int id, bool trackChanges)
    {
        return await FindByCondition(r => r.HabitId.Equals(habitId) && r.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    }

    public void CreateRepetition(Guid habitId, Repetition repetition)
    {
        repetition.HabitId = habitId;
        Create(repetition);
    }

    public void DeleteRepetition(Repetition repetition)
    {
        Delete(repetition);
    }
}
