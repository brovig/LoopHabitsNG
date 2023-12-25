using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IHabitService
{
    Task<IEnumerable<HabitDto>> GetAllHabitsAsync(string userId, bool trackChanges);
    Task<IEnumerable<HabitWithRepsDto>> GetAllHabitsWithRepsAsync(string userId, RepetitionParameters repetitionParameters, bool trackChanges);
    Task<HabitDto> GetHabitAsync(string userId, Guid habitId, bool trackChanges);
    Task<HabitDto> CreateHabitAsync(string userId, HabitForCreationDto habit);
    //Task<(IEnumerable<HabitDto> habits, string ids)> CreateHabitCollectionAsync(IEnumerable<HabitForCreationDto> habitCollection);
    Task DeleteHabitAsync(string userId, Guid habitId, bool trackChanges);
    Task UpdateHabitAsync(string userId, Guid habitId, HabitForUpdateDto habitForUpdate, bool trackChanges);
}