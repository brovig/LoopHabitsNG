using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IRepetitionService
{
    Task<IEnumerable<RepetitionDto>> GetRepetitionsAsync(string userId, Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges);
    Task<RepetitionDto> GetRepetitionAsync(string userId, Guid habitId, int id, bool trackChanges);
    Task<RepetitionDto> CreateRepetitionAsync(string userId, Guid habitId, RepetitionForCreationDto repetitionForCreation, bool trackChanges);
    Task UpdateRepetitionAsync(string userId, Guid habitId, int id, RepetitionForUpdateDto repetitionForUpdate, bool habitTrackChanges, bool repetitionTrackChanges);
    Task<IEnumerable<RepetitionDto>> CreateRepetitionCollectionAsync(string userId, Guid habitId, IEnumerable<RepetitionForCreationDto> repetitionsForCreation, bool trackChanges);
    Task DeleteRepetitionAsync(string userId, Guid habitId, int id, bool trackChanges);
}
