namespace Entities.Exceptions;
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() : base("User not found") { }
}
