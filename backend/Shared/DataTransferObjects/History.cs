namespace Shared.DataTransferObjects;
public record History
{
    public List<DateOnly> HistoryTimeStamps { get; set; } = new List<DateOnly>();
    public List<double> HistoryValues { get; set; } = new List<double>();
}
