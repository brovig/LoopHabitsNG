using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models;

public class Habit
{
    [Column("HabitId")]
    public Guid Id { get; set; }

    public bool IsArchived { get; set; }

    public int Color { get; set; }

    public string? Description { get; set; }

    public int FrequencyDensity { get; set; }

    public int FrequencyNumber { get; set; }

    public int Highlight { get; set; }

    [Required(ErrorMessage = "Name is a required field.")]
    public string? Name { get; set; }

    public int Position { get; set; }

    public DateTime ReminderTime { get; set; }

    public int ReminderDays { get; set; }

    public bool IsMeasurable { get; set; }

    public int Type { get; set; }

    public int TargetType { get; set; }

    public double TargetValue { get; set; }

    public string? Unit { get; set; }

    public string? Question { get; set; }
}
