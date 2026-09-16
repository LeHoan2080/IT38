using System.ComponentModel.DataAnnotations;

namespace AspNetMvcDemo.Models;

/**
 * Represents a note in the system.
 * This class is used to store note information in the database.
 */
public class Note
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public int UserId { get; set; }
}