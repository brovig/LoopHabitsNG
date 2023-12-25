namespace Shared.DataTransferObjects;

public record HabitWithRepsDto : HabitForManipulationDto
{
    public Guid Id { get; init; }
    public IEnumerable<RepetitionDto>? Repetitions { get; init; }
}
