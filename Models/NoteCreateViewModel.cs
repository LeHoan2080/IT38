using System.ComponentModel.DataAnnotations;

namespace AspNetMvcDemo.Models;

/**
 * Represents the view model for creating a note.
 * This class is used to capture user input when creating a new note.
 */
public class NoteCreateViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tiêu đề.")]
    [StringLength(
        200,
        ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự.")]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
    [Display(Name = "Nội dung")]
    public string Content { get; set; } = string.Empty;
}