using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IRepetitionService
{
    Task<IEnumerable<RepetitionDto>> GetRepetitionsAsync(Guid habitId, RepetitionParameters repetitionParameters, bool trackChanges);
    Task<RepetitionDto> GetRepetitionAsync(Guid habitId, int id, bool trackChanges);
    Task<RepetitionDto> CreateRepetitionAsync(Guid habitId, RepetitionForCreationDto repetitionForCreation, bool trackChanges);
    Task UpdateRepetitionAsync(Guid habitId, int id, RepetitionForUpdateDto repetitionForUpdate, bool habitTrackChanges, bool repetitionTrackChanges);
    Task<IEnumerable<RepetitionDto>> CreateRepetitionCollectionAsync(Guid habitId, IEnumerable<RepetitionForCreationDto> repetitionsForCreation, bool trackChanges);
    Task DeleteRepetitionAsync(Guid habitId, int id, bool trackChanges);
}
