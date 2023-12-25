using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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

    public async Task<IEnumerable<HabitDto>> GetAllHabitsAsync(string userId, bool trackChanges)
    {
        var habits = await _repository.Habit.GetAllHabitsAsync(userId, trackChanges);
        var habitsDto = _mapper.Map<IEnumerable<HabitDto>>(habits);
        return habitsDto;
    }

    public async Task<IEnumerable<HabitWithRepsDto>> GetAllHabitsWithRepsAsync(string userId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        if (!repetitionParameters.ValidDateRange) throw new DateRangeBadRequestException();

        var habits = await _repository.Habit.GetAllHabitsWithRepsAsync(userId, repetitionParameters, trackChanges);

        var habitsDto = _mapper.Map<IEnumerable<HabitWithRepsDto>>(habits);
        return habitsDto;
    }

    public async Task<HabitDto> CreateHabitAsync(string userId, HabitForCreationDto habit)
    {
        var habitEntity = _mapper.Map<Habit>(habit);

        habitEntity.UserId = userId;

        _repository.Habit.CreateHabit(habitEntity);
        await _repository.SaveAsync();

        var habitToReturn = _mapper.Map<HabitDto>(habitEntity);
        return habitToReturn;
    }

    //public async Task<(IEnumerable<HabitDto> habits, string ids)> CreateHabitCollectionAsync(IEnumerable<HabitForCreationDto> habitCollection)
    //{
    //    if (habitCollection is null)
    //        throw new HabitCollectionBadRequestException();

    //    var habitEntities = _mapper.Map<IEnumerable<Habit>>(habitCollection);
    //    foreach(var habit in habitEntities)
    //    {
    //        _repository.Habit.CreateHabit(habit);
    //    }

    //    await _repository.SaveAsync();

    //    var habitsToReturn = _mapper.Map<IEnumerable<HabitDto>>(habitEntities);
    //    var ids = string.Join(",", habitsToReturn.Select(h => h.Id));

    //    return (habits: habitsToReturn, ids: ids);
    //}      

    public async Task<HabitDto> GetHabitAsync(string userId, Guid habitId, bool trackChanges)
    {
        var habit = await GetHabitAndCheckIfItExists(userId, habitId, trackChanges);

        var habitDto = _mapper.Map<HabitDto>(habit);
        return habitDto;
    }

    public async Task UpdateHabitAsync(string userId, Guid habitId, HabitForUpdateDto habitForUpdate, bool trackChanges)
    {
        var habit = await GetHabitAndCheckIfItExists(userId, habitId, trackChanges);

        _mapper.Map(habitForUpdate, habit);
        await _repository.SaveAsync();
    }

    public async Task DeleteHabitAsync(string userId, Guid habitId, bool trackChanges)
    {
        var habit = await GetHabitAndCheckIfItExists(userId, habitId, trackChanges);

        _repository.Habit.DeleteHabit(habit);
        await _repository.SaveAsync();
    }


    private async Task<Habit> GetHabitAndCheckIfItExists(string userId, Guid id, bool trackChanges)
    {
        var habit = await _repository.Habit.GetHabitAsync(id, trackChanges);
        if (habit is null || habit.UserId != userId)
            throw new HabitNotFoundException(id);
        return habit;
    }
}
