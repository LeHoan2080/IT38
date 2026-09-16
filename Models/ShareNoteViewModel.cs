using System.ComponentModel.DataAnnotations;

namespace AspNetMvcDemo.Models;

/**
 * Represents the view model for sharing a note.
 * This class is used to capture user input when sharing a note with another user.
 */
public class ShareNoteViewModel
{
    public int NoteId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email người nhận.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    [Display(Name = "Email người nhận")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Cho phép chỉnh sửa")]
    public bool CanEdit { get; set; }
}