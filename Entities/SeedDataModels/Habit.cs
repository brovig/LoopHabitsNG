namespace Entities.SeedDataModels;

public partial class Habit
{
    public long Id { get; set; }

    public long? Archived { get; set; }

    public long? Color { get; set; }

    public string? Description { get; set; }

    public long? FreqDen { get; set; }

    public long? FreqNum { get; set; }

    public long? Highlight { get; set; }

    public string? Name { get; set; }

    public long? Position { get; set; }

    public long? ReminderHour { get; set; }

    public long? ReminderMin { get; set; }

    public long ReminderDays { get; set; }

    public long Type { get; set; }

    public long TargetType { get; set; }

    public double TargetValue { get; set; }

    public string Unit { get; set; } = null!;

    public string? Question { get; set; }

    public string? Uuid { get; set; }

    public virtual ICollection<Repetition> Repetitions { get; set; } = new List<Repetition>();
}
