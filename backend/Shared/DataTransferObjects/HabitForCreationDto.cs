namespace Shared.DataTransferObjects;

public record HabitForCreationDto : HabitForManipulationDto
{
    public IEnumerable<RepetitionForCreationDto>? Repetitions { get; init; }
}
