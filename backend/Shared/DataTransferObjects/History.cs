namespace Shared.DataTransferObjects;
public record History
{
    public List<string> HistoryTimeStamps { get; set; } = new List<string>();
    public List<int> HistoryValues { get; set; } = new List<int>();
}
