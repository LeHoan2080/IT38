namespace AspNetMvcDemo.Models;

/**
 * Represents the view model for a shared note.
 * This class is used to display information about a note that has been shared with the user.
 */
public class SharedNoteViewModel
{
    public int ShareId { get; set; }

    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool CanEdit { get; set; }

    public DateTime SharedAt { get; set; }
}