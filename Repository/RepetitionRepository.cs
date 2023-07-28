using Contracts;
using Entities.Models;

namespace Repository;

public class RepetitionRepository : RepositoryBase<Repetition>, IRepetitionRepository
{
    public RepetitionRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}
