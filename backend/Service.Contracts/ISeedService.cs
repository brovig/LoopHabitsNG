using Entities.SeedDataModels;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ISeedService
{
    Task<IEnumerable<Habit>> GetAllHabitsFromBackupAsync();
    Task<IEnumerable<Repetition>> GetAllRepetitionsFromBackupAsync();
    HabitForCreationDto MapHabitForCreation(Habit habit);
    IEnumerable<RepetitionForCreationDto> MapRepetitionsForCreation(IEnumerable<Repetition> repetitions);
}
