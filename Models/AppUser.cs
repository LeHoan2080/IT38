namespace AspNetMvcDemo.Models;

/**
 * Represents an application user in the system.
 * This class is used to store user information in the database.
 */
public class AppUser
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}