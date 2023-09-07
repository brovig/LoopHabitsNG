using Microsoft.AspNetCore.Identity;

namespace Entities.Models;
public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<Habit>? Habits { get; set; }
}
