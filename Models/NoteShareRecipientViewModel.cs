namespace AspNetMvcDemo.Models;

/**
 * Represents the view model for a note share recipient.
 * This class is used to display information about a user who has received a shared note.
 */
public class NoteShareRecipientViewModel
{
    public int ShareId { get; set; }

    public int NoteId { get; set; }

    public string NoteTitle { get; set; } = string.Empty;

    public string RecipientUsername { get; set; } = string.Empty;

    public string RecipientEmail { get; set; } = string.Empty;

    public bool CanEdit { get; set; }

    public DateTime SharedAt { get; set; }
}