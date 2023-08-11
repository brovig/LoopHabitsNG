﻿using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IHabitService
{
    Task<IEnumerable<HabitDto>> GetAllHabitsAsync(bool trackChanges);
    Task<HabitDto> GetHabitAsync(Guid habitId, bool trackChanges);
    Task<HabitDto> CreateHabitAsync(HabitForCreationDto habit);
    Task<IEnumerable<HabitDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<(IEnumerable<HabitDto> habits, string ids)> CreateHabitCollectionAsync(IEnumerable<HabitForCreationDto> habitCollection);
    Task DeleteHabitAsync(Guid habitId, bool trackChanges);
    Task UpdateHabitAsync(Guid habitId, HabitForUpdateDto habitForUpdate, bool trackChanges);
}