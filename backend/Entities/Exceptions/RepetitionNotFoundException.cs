namespace Entities.Exceptions;

public sealed class RepetitionNotFoundException : NotFoundException
{
    public RepetitionNotFoundException(int id) : base($"Repetition with id: {id} doesn't exist in the database.")
    {
    }
}
