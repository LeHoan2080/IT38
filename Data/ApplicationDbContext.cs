using AspNetMvcDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetMvcDemo.Data;

/**
 * Represents the database context for the application.
 * This class is used to interact with the database using Entity Framework Core.
 */
public class ApplicationDbContext : DbContext
{
    /**
     * Initializes a new instance of the ApplicationDbContext class.
     * @param options The options to be used by the DbContext.
     */
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; } = null!;
    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<NoteShare> NoteShares { get; set; } = null!;
}