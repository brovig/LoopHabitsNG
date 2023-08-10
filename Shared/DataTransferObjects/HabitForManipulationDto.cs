using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;
public abstract record HabitForManipulationDto
{
    public bool IsArchived { get; init; }

    public int Color { get; init; }

    public string? Description { get; init; }

    public int FrequencyDensity { get; init; }

    public int FrequencyNumber { get; init; }

    public int Highlight { get; set; }

    [Required(ErrorMessage = "Name is a required field.")]
    public string? Name { get; init; }

    public int Position { get; init; }

    public DateTime? ReminderTime { get; init; }

    public int ReminderDays { get; init; }

    public bool IsMeasurable { get; init; }

    public int Type { get; init; }

    public int TargetType { get; init; }

    public double TargetValue { get; init; }

    public string? Unit { get; init; }

    public string? Question { get; init; }
}
