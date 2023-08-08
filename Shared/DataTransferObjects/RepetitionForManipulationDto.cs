using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public abstract record RepetitionForManipulationDto
{
    [Required(ErrorMessage = "Repetition timestamp is a required field.")]
    public DateTime TimeStamp { get; init; }
    [Required(ErrorMessage = "Repetition value is a required field.")]
    public double Value { get; init; }
}
