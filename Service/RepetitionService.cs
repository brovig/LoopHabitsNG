using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    public async Task<RepetitionDto> CreateRepetitionAsync(Guid habitId, RepetitionForCreationDto repetitionForCreation, bool trackChanges)
    {
        await CheckIfHabitExists(habitId, trackChanges);

        var repetitionEntity = _mapper.Map<Repetition>(repetitionForCreation);

        _repository.Repetition.CreateRepetition(habitId, repetitionEntity);
        await _repository.SaveAsync();

        var repetitionToReturn = _mapper.Map<RepetitionDto>(repetitionEntity);
        return repetitionToReturn;
    }

    public async Task<IEnumerable<RepetitionDto>> CreateRepetitionCollectionAsync(Guid habitId, IEnumerable<RepetitionForCreationDto> repetitionsForCreation, bool trackChanges)
    {
        var result = new List<RepetitionDto>();
        foreach(var repetition in repetitionsForCreation)
        {
            var addedRepetition = await CreateRepetitionAsync(habitId, repetition, trackChanges: false);
            result.Add(addedRepetition);
        }
        return result;
    }

    public async Task<IEnumerable<RepetitionDto>> GetAllRepetitionsAsync(Guid habitId, bool trackChanges)
    {
        await CheckIfHabitExists(habitId, trackChanges);

        var repetitions = await _repository.Repetition.GetAllRepetitionsAsync(habitId, trackChanges);

        var repetitionsDto = _mapper.Map<IEnumerable<RepetitionDto>>(repetitions);
        return repetitionsDto;
    }

    public async Task<RepetitionDto> GetRepetitionAsync(Guid habitId, int id, bool trackChanges)
    {
        await CheckIfHabitExists(habitId, trackChanges);

        var repetitionEntity = await GetRepetitionAndCheckIfItExists(habitId, id, trackChanges);

        var repetitionDto = _mapper.Map<RepetitionDto>(repetitionEntity);
        return repetitionDto;
    }

    public async Task UpdateRepetitionAsync(Guid habitId, int id, RepetitionForUpdateDto repetitionForUpdate, bool habitTrackChanges, bool repetitionTrackChanges)
    {
        await CheckIfHabitExists(habitId, habitTrackChanges);

        var repetitionEntity = await GetRepetitionAndCheckIfItExists(habitId, id, repetitionTrackChanges);

        _mapper.Map(repetitionForUpdate, repetitionEntity);
        await _repository.SaveAsync();
    }

    public async Task DeleteRepetitionAsync(Guid habitId, int id, bool trackChanges)
    {
        await CheckIfHabitExists(habitId, trackChanges);

        var repetition = await GetRepetitionAndCheckIfItExists(habitId, id, trackChanges);
        _repository.Repetition.DeleteRepetition(repetition);
        await _repository.SaveAsync();
    }


    // helper methods
    private async Task CheckIfHabitExists(Guid habitId, bool trackChanges)
    {
        var habit = await _repository.Habit.GetHabitAsync(habitId, trackChanges);
        if (habit is null)
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
