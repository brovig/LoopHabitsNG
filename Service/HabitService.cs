using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class HabitService : IHabitService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public HabitService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<HabitDto> CreateHabitAsync(HabitDto habit)
    {
        var habitEntity = _mapper.Map<Habit>(habit);

        _repository.Habit.CreateHabit(habitEntity);
        await _repository.SaveAsync();

        var habitToReturn = _mapper.Map<HabitDto>(habitEntity);
        return habitToReturn;
    }

    public async Task<(IEnumerable<HabitDto> habits, string ids)> CreateHabitCollectionAsync(IEnumerable<HabitDto> habitCollection)
    {
        if (habitCollection is null)
            throw new HabitCollectionBadRequestException();

        var habitEntities = _mapper.Map<IEnumerable<Habit>>(habitCollection);
        foreach(var habit in habitEntities)
        {
            _repository.Habit.CreateHabit(habit);
        }

        await _repository.SaveAsync();

        var habitsToReturn = _mapper.Map<IEnumerable<HabitDto>>(habitEntities);
        var ids = string.Join(",", habitsToReturn.Select(h => h.Id));

        return (habits: habitsToReturn, ids: ids);
    }

    public Task DeleteCompanyAsync(Guid habitId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<HabitDto>> GetAllHabitsAsync(bool trackChanges)
    {
        var habits = await _repository.Habit.GetAllHabitsAsync(trackChanges);
        var habitsDto = _mapper.Map<IEnumerable<HabitDto>>(habits);
        return habitsDto;
    }

    public Task<IEnumerable<HabitDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<HabitDto> GetHabitAsync(Guid habitId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCompanyAsync(Guid habitId, HabitDto companyForUpdate, bool trackChanges)
    {
        throw new NotImplementedException();
    }
}
