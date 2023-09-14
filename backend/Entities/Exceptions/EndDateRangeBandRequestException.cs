namespace Entities.Exceptions;

public sealed class EndDateRangeBandRequestException : BadRequestException
{
    public EndDateRangeBandRequestException() : base("End date can't be less than start date.")
    {
    }
}
