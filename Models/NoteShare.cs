namespace AspNetMvcDemo.Models;

/**
 * Represents a note share in the system.
 * This class is used to store information about shared notes in the database.
 */
public class NoteShare
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    public int UserId { get; set; }

    public bool CanEdit { get; set; }

    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
}