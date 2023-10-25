namespace Shared.DataTransferObjects;

public record ScoresDto
{ 
    public List<string> TimeStamps { get; set; } = new List<string>();
    public List<double> Values { get; set; } = new List<double>();
}
    
