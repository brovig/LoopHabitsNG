using Contracts;
using Service.Contracts;
using Entities.SeedDataModels;

namespace Service;
internal sealed class SeedService : ISeedService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public SeedService(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Habit>> GetAllHabitsFromBackupAsync()
    {
        var habits = await _repository.SqliteBackupRepository.GetAllHabitsFromBackupAsync();
        return habits;
    }

    public async Task<IEnumerable<Repetition>> GetAllRepetitionsFromBackupAsync()
    {
        var repetitions = await _repository.SqliteBackupRepository.GetAllRepetitionsFromBackupAsync();
        return repetitions;
    }
}
