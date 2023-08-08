using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IHabitService
{
    Task<IEnumerable<HabitDto>> GetAllHabitsAsync(bool trackChanges);
    Task<HabitDto> GetHabitAsync(Guid habitId, bool trackChanges);
    Task<HabitDto> CreateHabitAsync(HabitDto habit);
    Task<IEnumerable<HabitDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<(IEnumerable<HabitDto> habits, string ids)> CreateHabitCollectionAsync(IEnumerable<HabitDto> habitCollection);
    Task DeleteCompanyAsync(Guid habitId, bool trackChanges);
    Task UpdateCompanyAsync(Guid habitId, HabitDto companyForUpdate, bool trackChanges);
}