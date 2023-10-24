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
        foreach (var repetition in repetitionsForCreation)
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



    public async Task<ScoresDto> GetHabitScoresAsync(string userId, Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        Habit habit = await GetHabitAndCheckIfItExists(userId, habitId, trackChanges);

        var repetitions = await _repository.Repetition.GetRepetitionsAsync(habitId, repetitionParameters, trackChanges);

        var scoresDto = GetScoresListFromRepetitions(repetitions, repetitionParameters.EndDate, habit);

        return scoresDto;
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

    private async Task<Habit> GetHabitAndCheckIfItExists(string userId, Guid id, bool trackChanges)
    {
        var habit = await _repository.Habit.GetHabitAsync(id, trackChanges);
        if (habit is null || habit.UserId != userId)
            throw new HabitNotFoundException(id);
        return habit;
    }

    private async Task<Repetition> GetRepetitionAndCheckIfItExists(Guid habitId, int id, bool repetitionTrackChanges)
    {
        var repetitionEntity = await _repository.Repetition.GetRepetitionAsync(habitId, id, repetitionTrackChanges);
        if (repetitionEntity is null)
            throw new RepetitionNotFoundException(id);
        return repetitionEntity;
    }

    // Repetitions collection processed to get scores collection:
    // 1. Get rep values for every day in time period (assigning 0 to abscent reps)
    // 2. Calculate scores
    private ScoresDto GetScoresListFromRepetitions(IEnumerable<Repetition> repetitions, DateTime endDate, Habit habit)
    {
        // Get starting date (first repetition by time, the last in the list)
        DateTime startDate = repetitions.Last().Timestamp;

        // Generate sequence of days from start to end
        int amountOfDays = (endDate - startDate).Days + 1;
        var dates = Enumerable.Range(0, amountOfDays).Select(d => startDate.AddDays(d)).ToList();

        // Populate new collection with values for every day
        var repsForAllDays = from d in dates
                             join r in repetitions on d equals r.Timestamp into grp
                             from r in grp.DefaultIfEmpty(new Repetition { Timestamp = d, Value = 0.0 })
                             select r.Value;
        var values = repsForAllDays.ToList();

        // Populate scores collection
        var scores = new ScoresDto { TimeStamps = dates };
        double rollingSum = 0.0;
        int numerator = habit.FrequencyNumber;
        int denominator = habit.FrequencyDensity;
        double frequency = (double)numerator / denominator;
        bool isNumerical = habit.Type == 1;
        bool isAtMost = habit.TargetType == 1;
        int entrySkipValue = 3;
        double yesManualValue = 2;
        double percentageCompleted = 0.0;

        if (!isNumerical && frequency < 1.0)
        {
            numerator *= 2;
            denominator *= 2;
        }

        double previousValue = isNumerical && isAtMost ? 1.0 : 0.0;

        for (int i = 0; i < values.Count; i++)
        {            
            if (isNumerical)
            {
                rollingSum += values[i];
                if (i - denominator >= 0)
                    rollingSum -= values[i - denominator];
                double normalizedRollingSum = rollingSum / 1000;
                if (values[i] != entrySkipValue)
                {
                    if (!isAtMost)
                    {
                        if (habit.TargetValue > 0)
                            percentageCompleted = Math.Min(1.0, normalizedRollingSum / habit.TargetValue);
                        else
                            percentageCompleted = 1.0;
                    }
                    else
                    {
                        if (habit.TargetValue > 0)
                        {
                            percentageCompleted = 1 - (normalizedRollingSum - habit.TargetValue) / habit.TargetValue;
                            if (percentageCompleted < 0)
                                percentageCompleted = 0;
                            if (percentageCompleted > 1.0)
                                percentageCompleted = 1.0;
                        }
                        else
                        {
                            if (normalizedRollingSum > 0)
                                percentageCompleted = 0;
                            else
                                percentageCompleted = 1.0;
                        }
                    }

                    previousValue = CalculateScoreForRep(frequency, previousValue, percentageCompleted);
                }
            }
            else
            {
                if (values[i] == yesManualValue)
                        rollingSum += 1.0;
                if (i - denominator >= 0)
                {
                    if (values[i - denominator] == yesManualValue)
                        rollingSum -= 1.0;
                }
                if (values[i] != entrySkipValue)
                {
                    percentageCompleted = Math.Min(1.0, rollingSum / numerator);
                    previousValue = CalculateScoreForRep(frequency, previousValue, percentageCompleted);
                }
            }

            scores.Values.Add(previousValue);
        }

        return scores;
    }

    private double CalculateScoreForRep(double frequency, double previousScore, double checkmarkValue)
    {
        double multiplier = Math.Pow(0.5, Math.Sqrt(frequency) / 13.0);
        double score = previousScore * multiplier;
        score += checkmarkValue * (1 - multiplier);
        return score;
    }
}
