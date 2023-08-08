using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepetitionRepository : RepositoryBase<Repetition>, IRepetitionRepository
{
    public RepetitionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Repetition>> GetAllRepetitionsAsync(Guid habitId, bool trackChanges)
    {
        return await FindByCondition(r => r.HabitId.Equals(habitId), trackChanges).ToListAsync();
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
