namespace Shared.DataTransferObjects;

public record HabitStatisticsDto
{
    public Scores Scores { get; set; } = new Scores();
    public int TotalReps { get; set; }
    public History History { get; set; } = new History();
    public List<DateOnly> Dates {  get; set; } = new List<DateOnly>();
    public List<bool> CalendarMarks { get; set; } = new List<bool>();
}