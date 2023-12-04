namespace Shared.DataTransferObjects;
public record Scores
{
    public List<string> ScoreTimeStamps { get; set; } = new List<string>();
    public List<double> ScoreValues { get; set; } = new List<double>();
}
