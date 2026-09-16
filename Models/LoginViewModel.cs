using System.ComponentModel.DataAnnotations;

namespace AspNetMvcDemo.Models;

/**
 * Represents the view model for the login page.
 * This class is used to capture user input when logging in.
 */
public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}