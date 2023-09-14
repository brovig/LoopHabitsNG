using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

internal sealed class RepetitionService : IRepetitionService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public RepetitionService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<RepetitionDto> CreateRepetitionAsync(string userId, Guid habitId, RepetitionForCreationDto repetitionForCreation, bool trackChanges)
    {
        await CheckIfHabitExists(userId, habitId, trackChanges);

        var repetitionEntity = _mapper.Map<Repetition>(repetitionForCreation);

        _repository.Repetition.CreateRepetition(habitId, repetitionEntity);
        await _repository.SaveAsync();

        var repetitionToReturn = _mapper.Map<RepetitionDto>(repetitionEntity);
        return repetitionToReturn;
    }

    public async Task<IEnumerable<RepetitionDto>> CreateRepetitionCollectionAsync(string userId, Guid habitId, IEnumerable<RepetitionForCreationDto> repetitionsForCreation, bool trackChanges)
    {
        var result = new List<RepetitionDto>();
        foreach(var repetition in repetitionsForCreation)
        {
            var addedRepetition = await CreateRepetitionAsync(userId, habitId, repetition, trackChanges: false);
            result.Add(addedRepetition);
        }
        return result;
    }

    public async Task<IEnumerable<RepetitionDto>> GetRepetitionsAsync(string userId, Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        await CheckIfHabitExists(userId, habitId, trackChanges);

        var repetitions = await _repository.Repetition.GetRepetitionsAsync(habitId, repetitionParameters, trackChanges);

        var repetitionsDto = _mapper.Map<IEnumerable<RepetitionDto>>(repetitions);
        return repetitionsDto;
    }

    public async Task<RepetitionDto> GetRepetitionAsync(string userId, Guid habitId, int id, bool trackChanges)
    {
        await CheckIfHabitExists(userId, habitId, trackChanges);

        var repetitionEntity = await GetRepetitionAndCheckIfItExists(habitId, id, trackChanges);

        var repetitionDto = _mapper.Map<RepetitionDto>(repetitionEntity);
        return repetitionDto;
    }

    public async Task UpdateRepetitionAsync(string userId, Guid habitId, int id, RepetitionForUpdateDto repetitionForUpdate, bool habitTrackChanges, bool repetitionTrackChanges)
    {
        await CheckIfHabitExists(userId, habitId, habitTrackChanges);

        var repetitionEntity = await GetRepetitionAndCheckIfItExists(habitId, id, repetitionTrackChanges);

        _mapper.Map(repetitionForUpdate, repetitionEntity);
        await _repository.SaveAsync();
    }

    public async Task DeleteRepetitionAsync(string userId, Guid habitId, int id, bool trackChanges)
    {
        await CheckIfHabitExists(userId, habitId, trackChanges);

        var repetition = await GetRepetitionAndCheckIfItExists(habitId, id, trackChanges);
        _repository.Repetition.DeleteRepetition(repetition);
        await _repository.SaveAsync();
    }


    // helper methods
    private async Task CheckIfHabitExists(string userId, Guid habitId, bool trackChanges)
    {
        var habit = await _repository.Habit.GetHabitAsync(habitId, trackChanges);
        if (habit is null || habit.UserId != userId)
            throw new HabitNotFoundException(habitId);
    }

    private async Task<Repetition> GetRepetitionAndCheckIfItExists(Guid habitId, int id, bool repetitionTrackChanges)
    {
        var repetitionEntity = await _repository.Repetition.GetRepetitionAsync(habitId, id, repetitionTrackChanges);
        if (repetitionEntity is null)
            throw new RepetitionNotFoundException(id);
        return repetitionEntity;
    }
}
