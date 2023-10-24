namespace Shared.DataTransferObjects;

public record ScoresDto
{ 
    public List<DateTime> TimeStamps { get; set; } = new List<DateTime>();
    public List<double> Values { get; set; } = new List<double>();
}
    
