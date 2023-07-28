using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IHabitRepository> _habitRepository;
    private readonly Lazy<IRepetitionRepository> _repetitionRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _habitRepository = new Lazy<IHabitRepository>(() => new HabitRepository(repositoryContext));
        _repetitionRepository = new Lazy<IRepetitionRepository>(() => new RepetitionRepository(repositoryContext));
    }

    public IHabitRepository HabitRepository => _habitRepository.Value;

    public IRepetitionRepository RepetitionRepository => _repetitionRepository.Value;

    public void Save() => _repositoryContext.SaveChanges();
}
