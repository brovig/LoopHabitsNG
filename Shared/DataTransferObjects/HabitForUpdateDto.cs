namespace Shared.DataTransferObjects;

public record HabitForUpdateDto
{
    public IEnumerable<RepetitionForCreationDto>? Repetitions { get; init; }

}
