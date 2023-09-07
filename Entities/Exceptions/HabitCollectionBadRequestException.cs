namespace Entities.Exceptions;
public sealed class HabitCollectionBadRequestException : BadRequestException
{
    public HabitCollectionBadRequestException() : base("Habit collection sent from a client is null.")
    {
    }
}
