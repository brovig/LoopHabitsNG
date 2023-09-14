namespace Entities.Exceptions;

public sealed class HabitNotFoundException : NotFoundException
{
    public HabitNotFoundException(Guid habitId) : base($"The habit with id: {habitId} doesn't exist in the database.")
    {
    }
}
