using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Globalization;

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



    public async Task<HabitStatisticsDto> GetHabitStatisticsAsync(string userId, Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges)
    {
        Habit habit = await GetHabitAndCheckIfItExists(userId, habitId, trackChanges);

        var repetitions = await _repository.Repetition.GetRepetitionsAsync(habitId, repetitionParameters, trackChanges);

        var habitStatsDto = GetStatsFromRepetitions(repetitions, repetitionParameters.EndDate, habit);

        return habitStatsDto;
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
    private HabitStatisticsDto GetStatsFromRepetitions(IEnumerable<Repetition> repetitions, DateTime endDate, Habit habit)
    {
        int repetitionsCount = repetitions.Where(r => r.Value == 2).Count();
        if (repetitions.Count() == 0)
        {
            return new HabitStatisticsDto
            {
                Scores = new Scores
                {
                    ScoreTimeStamps = new List<string> { endDate.Day.ToString() },
                    ScoreValues = new List<double> { 0.0 }
                },
                TotalReps = 0,
                History = new History
                {
                    HistoryTimeStamps = new List<DateOnly> { DateOnly.FromDateTime(endDate) },
                    HistoryValues = new List<double> { 0.0 }
                }
            };
        }

        // Get starting date (first repetition by time, the last in the list)
        DateTime startDate = repetitions.Last().Timestamp;

        // Generate sequence of days from start to end
        int amountOfDays = (endDate - startDate).Days + 1;
        var dates = Enumerable.Range(0, amountOfDays).Select(d => startDate.AddDays(d));

        // Populate new collection with values for every day
        var repsForAllDays = from d in dates
                             join r in repetitions on d equals r.Timestamp into grp
                             from r in grp.DefaultIfEmpty(new Repetition { Timestamp = d, Value = 0.0 })
                             select r;
        var repValuesForAllDays = repsForAllDays.Select(r => r.Value);
        // Formatting dates as they will be present on client: each date projected into a string. 
        // If the date is the first date of the month, check if it is also the first day of year. If so, return the year.
        // Otherwise, return first three letters of the month name. 
        // In all other cases return just the day of month as string
        var formattedDates = dates.Select(d => d.Day == 1 ? (d.Month == 1 ? d.Year.ToString() : d.ToString("MMM", CultureInfo.GetCultureInfo("en-US"))) : d.Day.ToString()).ToList();
        var scoreValues = repValuesForAllDays.ToList();

        // Populate scores collection
        var habitStatistics = new HabitStatisticsDto
        {
            TotalReps = repetitionsCount
        };
        var scores = new Scores { ScoreTimeStamps = formattedDates };
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

        for (int i = 0; i < scoreValues.Count; i++)
        {
            if (isNumerical)
            {
                rollingSum += scoreValues[i];
                if (i - denominator >= 0)
                    rollingSum -= scoreValues[i - denominator];
                double normalizedRollingSum = rollingSum / 1000;
                if (scoreValues[i] != entrySkipValue)
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
                if (scoreValues[i] == yesManualValue)
                        rollingSum += 1.0;
                if (i - denominator >= 0)
                {
                    if (scoreValues[i - denominator] == yesManualValue)
                        rollingSum -= 1.0;
                }
                if (scoreValues[i] != entrySkipValue)
                {
                    percentageCompleted = Math.Min(1.0, rollingSum / numerator);
                    previousValue = CalculateScoreForRep(frequency, previousValue, percentageCompleted);
                }
            }

            scores.ScoreValues.Add(previousValue * 100);
        }
        habitStatistics.Scores = scores;

        // Populate history (amount of repetitions per week)
        var historyByWeek = GetHistory(repsForAllDays, isNumerical);
        habitStatistics.History = historyByWeek;

        return habitStatistics;
    }

    private double CalculateScoreForRep(double frequency, double previousScore, double checkmarkValue)
    {
        double multiplier = Math.Pow(0.5, Math.Sqrt(frequency) / 13.0);
        double score = previousScore * multiplier;
        score += checkmarkValue * (1 - multiplier);
        return score;
    }

    private History GetHistory(IEnumerable<Repetition> repetitions, bool isNumerical)
    {
        var historyData = repetitions
                            .GroupBy(r => DateOnly.FromDateTime(r.Timestamp).AddDays(-(int)r.Timestamp.DayOfWeek + (int)DayOfWeek.Monday))
                            .Select(g => new
                            {
                                StartOfWeek = g.Key,
                                HistoryValue = isNumerical ? g.Select(r => r.Value).Sum() / 1000 : g.Where(r => r.Value != 0).Count()
                            })
                            .ToList();

        var history = new History();
        history.HistoryTimeStamps.AddRange(historyData.Select(h => h.StartOfWeek));
        history.HistoryValues.AddRange(historyData.Select(h => h.HistoryValue));
        
        return history;
    }
}
