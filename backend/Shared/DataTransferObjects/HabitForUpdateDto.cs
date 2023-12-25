namespace Shared.DataTransferObjects;

public record HabitForUpdateDto : HabitForManipulationDto
{
    public IEnumerable<RepetitionForCreationDto>? Repetitions { get; init; }
}
