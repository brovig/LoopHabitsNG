using Contracts;
using Service.Contracts;
using Entities.SeedDataModels;
using AutoMapper;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class SeedService : ISeedService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public SeedService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Habit>> GetAllHabitsFromBackupAsync()
    {
        var habits = await _repository.SqliteBackup.GetAllHabitsFromBackupAsync();
        return habits;
    }

    public async Task<IEnumerable<Repetition>> GetAllRepetitionsFromBackupAsync()
    {
        var repetitions = await _repository.SqliteBackup.GetAllRepetitionsFromBackupAsync();
        return repetitions;
    }

    public IEnumerable<RepetitionForCreationDto> MapRepetitionsForCreation(IEnumerable<Repetition> repetitions)
    {
        var result = _mapper.Map<IEnumerable<RepetitionForCreationDto>>(repetitions);
        return result;
    }

    public HabitForCreationDto MapHabitForCreation(Habit habit)
    {
        var result = _mapper.Map<HabitForCreationDto>(habit);
        return result;
    }
}
