using System.Security.Claims;
using AspNetMvcDemo.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetMvcDemo.Models;

namespace AspNetMvcDemo.Controllers;

/**
 * Controller for managing notes.
 * This controller handles requests related to notes, including listing notes for the authenticated user.
 */
[Authorize]

/**
 * Controller for managing notes.
 * This controller handles requests related to notes, including listing notes for the authenticated user.
 */
public class NotesController : Controller
{
    private readonly ApplicationDbContext _context;

    /**
     * Initializes a new instance of the NotesController class.
     * @param context The application database context.
     */
    public NotesController(ApplicationDbContext context)
    {
        _context = context;
    }

    /**
     * Displays a list of notes for the authenticated user.
     * @return A view displaying the user's notes.
     */
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        // Retrieve notes for the authenticated user from the database
        var notes = await _context.Notes
            .Where(note => note.UserId == userId)
            .OrderByDescending(note => note.CreatedAt)
            .ToListAsync();

        return View(notes);
    }

    /**
     * Displays the form for creating a new note.
     * @return A view displaying the note creation form.
     */
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /**
     * Handles the submission of the note creation form.
     * @param model The view model containing the note data.
     * @return A redirect to the notes index if successful, or the creation view with validation errors.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoteCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        var note = new Note
        {
            Title = model.Title.Trim(),
            Content = model.Content.Trim(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    /**
     * Displays the form for editing an existing note.
     * @param id The ID of the note to edit.
     * @param fromShared Whether the note is being edited from a shared note.
     * @return A view displaying the note editing form, or a NotFound result if the note does not exist or does not belong to the user.
     */
    [HttpGet]
    public async Task<IActionResult> Edit(
        int id,
        bool fromShared = false)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        Note? note = await _context.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == id &&
                (
                    note.UserId == userId ||
                    _context.NoteShares.Any(share =>
                        share.NoteId == id &&
                        share.UserId == userId)
                ));

        if (note is null)
        {
            return NotFound();
        }

        bool canEdit =
            note.UserId == userId ||
            await _context.NoteShares.AnyAsync(share =>
                share.NoteId == id &&
                share.UserId == userId &&
                share.CanEdit);

        var model = new NoteEditViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            CanEdit = canEdit,
            FromShared = fromShared
        };

        return View(model);
    }

    /**
     * Handles the submission of the note editing form.
     * @param id The ID of the note being edited.
     * @param model The view model containing the updated note data.
     * @return A redirect to the notes index if successful, or the editing view with validation errors.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        NoteEditViewModel model,
        bool fromShared = false)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        Note? note = await _context.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == id &&
                (
                    note.UserId == userId ||
                    _context.NoteShares.Any(share =>
                        share.NoteId == id &&
                        share.UserId == userId &&
                        share.CanEdit)
                ));

        if (note is null)
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            model.CanEdit = true;
            model.FromShared = fromShared;
            return View(model);
        }

        note.Title = model.Title.Trim();
        note.Content = model.Content.Trim();
        note.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return fromShared
            ? RedirectToAction(nameof(SharedWithMe))
            : RedirectToAction(nameof(Index));
    }

    /**
     * Handles the deletion of a note.
     * @param id The ID of the note to delete.
     * @return A redirect to the notes index if successful, or a NotFound result if the note does not exist or does not belong to the user.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        Note? note = await _context.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == id &&
                note.UserId == userId);

        if (note is null)
        {
            return NotFound();
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /**
     * Displays the form for sharing a note with another user.
     * @param id The ID of the note to share.
     * @return A view displaying the note sharing form, or a NotFound result if the note does not exist or does not belong to the user.
     */
    [HttpGet]
    public async Task<IActionResult> Share(int id)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        Note? note = await _context.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == id &&
                note.UserId == userId);

        if (note is null)
        {
            return NotFound();
        }

        return View(new ShareNoteViewModel
        {
            NoteId = note.Id
        });
    }

    /**
     * Handles the submission of the note sharing form.
     * @param model The view model containing the sharing information.
     * @return A redirect to the notes index if successful, or the sharing view with validation errors.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Share(
        ShareNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int ownerId))
        {
            return Challenge();
        }

        Note? note = await _context.Notes
            .FirstOrDefaultAsync(note =>
                note.Id == model.NoteId &&
                note.UserId == ownerId);

        if (note is null)
        {
            return NotFound();
        }

        string email = model.Email.Trim().ToLowerInvariant();

        AppUser? recipient = await _context.Users
            .FirstOrDefaultAsync(user => user.Email == email);

        if (recipient is null)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Không tìm thấy người dùng với email này.");

            return View(model);
        }

        if (recipient.Id == ownerId)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Bạn không thể chia sẻ ghi chú cho chính mình.");

            return View(model);
        }

        bool alreadyShared = await _context.NoteShares
            .AnyAsync(share =>
                share.NoteId == model.NoteId &&
                share.UserId == recipient.Id);

        if (alreadyShared)
        {
            ModelState.AddModelError(
                nameof(model.Email),
                "Ghi chú đã được chia sẻ cho người dùng này.");

            return View(model);
        }

        var noteShare = new NoteShare
        {
            NoteId = model.NoteId,
            UserId = recipient.Id,
            CanEdit = model.CanEdit
        };

        _context.NoteShares.Add(noteShare);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    /**
     * Displays a list of notes that have been shared with the authenticated user.
     * @return A view displaying the notes shared with the user.
     */
    [HttpGet]
    public async Task<IActionResult> SharedWithMe()
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        var notes = await (
            from share in _context.NoteShares
            join note in _context.Notes
                on share.NoteId equals note.Id
            where share.UserId == userId
            orderby share.SharedAt descending
            select new SharedNoteViewModel
            {
                ShareId = share.Id,
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CanEdit = share.CanEdit,
                SharedAt = share.SharedAt
            })
            .ToListAsync();

        return View(notes);
    }

    /**
     * Handles the request to leave a shared note.
     * @param shareId The ID of the note share to leave.
     * @return A redirect to the shared notes view if successful, or a NotFound result if the share does not exist or does not belong to the user.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LeaveShare(int shareId)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
        {
            return Challenge();
        }

        NoteShare? share = await _context.NoteShares
            .FirstOrDefaultAsync(item =>
                item.Id == shareId &&
                item.UserId == userId);

        if (share is null)
        {
            return NotFound();
        }

        _context.NoteShares.Remove(share);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(SharedWithMe));
    }

    /**
     * Displays a list of recipients for a note that the authenticated user has shared.
     * @param noteId The ID of the note for which to display recipients.
     * @return A view displaying the recipients of the shared note, or a NotFound result if the note does not exist or does not belong to the user.
     */
    [HttpGet]
    public async Task<IActionResult> SharedByMe(int noteId)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int ownerId))
        {
            return Challenge();
        }

        bool ownsNote = await _context.Notes
            .AnyAsync(note =>
                note.Id == noteId &&
                note.UserId == ownerId);

        if (!ownsNote)
        {
            return NotFound();
        }

        var recipients = await (
            from share in _context.NoteShares
            join note in _context.Notes
                on share.NoteId equals note.Id
            join user in _context.Users
                on share.UserId equals user.Id
            where share.NoteId == noteId
            orderby share.SharedAt descending
            select new NoteShareRecipientViewModel
            {
                ShareId = share.Id,
                NoteId = note.Id,
                NoteTitle = note.Title,
                RecipientUsername = user.Username,
                RecipientEmail = user.Email,
                CanEdit = share.CanEdit,
                SharedAt = share.SharedAt
            })
            .ToListAsync();

        return View(recipients);
    }

    /**
     * Handles the request to revoke a share.
     * @param shareId The ID of the note share to revoke.
     * @return A redirect to the shared notes view if successful, or a NotFound result if the share does not exist or does not belong to the user.
     */
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RevokeShare(int shareId)
    {
        string? userIdValue = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int ownerId))
        {
            return Challenge();
        }

        NoteShare? share = await (
            from currentShare in _context.NoteShares
            join note in _context.Notes
                on currentShare.NoteId equals note.Id
            where currentShare.Id == shareId &&
                  note.UserId == ownerId
            select currentShare)
            .FirstOrDefaultAsync();

        if (share is null)
        {
            return NotFound();
        }

        int noteId = share.NoteId;

        _context.NoteShares.Remove(share);
        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(SharedByMe),
            new { noteId });
    }
}