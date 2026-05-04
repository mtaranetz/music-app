using CatalogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Track> Tracks { get; set; }

    
}