namespace Shared.DataTransferObjects;

public record HabitDto(
    Guid Id,
    bool IsArchived,
    int Color,
    string Description,
    int FrequencyDensity,
    int FrequencyNumber,
    int Highlight,
    string Name,
    int Position,
    DateTime? ReminderTime,
    int ReminderDays,
    bool IsMeasurable,
    int Type,
    int TargetType,
    int TargetValue,
    string Unit,
    string Question);