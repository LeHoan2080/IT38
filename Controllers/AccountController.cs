using AspNetMvcDemo.Data;
using AspNetMvcDemo.Models;
using AspNetMvcDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNetMvcDemo.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    /**
     * Initializes a new instance of the AccountController class.
     * @param context The database context to be used by the controller.
     */
    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    /**
     * Displays the registration form to the user.
     * @return The registration view.
     */
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    /**
     * Handles the user registration process.
     * @param model The view model containing user input for registration.
     * @return A redirect to the success page if registration is successful, or the registration view with validation errors.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        string username = model.Username.Trim();
        string email = model.Email.Trim().ToLowerInvariant();

        bool emailExists = await _context.Users
            .AnyAsync(user => user.Email == email);

        if (emailExists){
            ModelState.AddModelError(
                nameof(model.Email),
                "Email này đã được đăng ký.");
            return View(model);
        }

        var user = new AppUser
        {
            Username = username,
            Email = email,
            PasswordHash = PasswordService.HashPassword(model.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(RegisterSuccess));
    }

    /**
     * Displays the registration success page to the user.
     * @return The registration success view.
     */
    [HttpGet]
    public IActionResult RegisterSuccess()
    {
        return View();
    }

    /**
     * Displays the login form to the user.
     * @return The login view.
     */
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    /**
     * Handles the user login process.
     * @param model The view model containing user input for login.
     * @return A redirect to the home page if login is successful, or the login view with validation errors.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }


        string email = model.Email.Trim().ToLowerInvariant();

        AppUser? user = await _context.Users
            .FirstOrDefaultAsync(item => item.Email == email);

        if (user is null ||
            !PasswordService.VerifyPassword(
                model.Password,
                user.PasswordHash))
        {
            ModelState.AddModelError(
                string.Empty,
                "Email hoặc mật khẩu không đúng.");

            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        // Sign in the user with cookie authentication
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal);

        // Redirect to the home page after successful login
        return RedirectToAction(
            "Index",
            "Home");
    }

    /**
     * Handles the user logout process.
     * @return A redirect to the login page after logging out.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Home");
    }
}