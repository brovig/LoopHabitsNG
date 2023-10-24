namespace Shared.RequestFeatures;

public class RepetitionParameters : RequestParameters
{
    public DateTime StartDate { get; set; } = DateTime.MinValue;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;

    public bool ValidDateRange => EndDate > StartDate;
}
