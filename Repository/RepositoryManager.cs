using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly SqliteBackupContext _sqliteBackupContext;
    private readonly Lazy<IHabitRepository> _habitRepository;
    private readonly Lazy<IRepetitionRepository> _repetitionRepository;
    private readonly Lazy<ISqliteBackupRepository> _sqliteBackupRepository;

    public RepositoryManager(RepositoryContext repositoryContext, SqliteBackupContext sqliteBackupContext)
    {
        _repositoryContext = repositoryContext;
        _sqliteBackupContext = sqliteBackupContext;
        _habitRepository = new Lazy<IHabitRepository>(() => new HabitRepository(repositoryContext));
        _repetitionRepository = new Lazy<IRepetitionRepository>(() => new RepetitionRepository(repositoryContext));
        _sqliteBackupRepository = new Lazy<ISqliteBackupRepository>(() => new SqliteBackupRepository(sqliteBackupContext));
    }

    public IHabitRepository HabitRepository => _habitRepository.Value;

    public IRepetitionRepository RepetitionRepository => _repetitionRepository.Value;

    public ISqliteBackupRepository SqliteBackupRepository => _sqliteBackupRepository.Value;

    public void Save() => _repositoryContext.SaveChanges();
}
