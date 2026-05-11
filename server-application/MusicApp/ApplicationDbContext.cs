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
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<PlaylistTrack> PlaylistTracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Track>(entity =>
        {
            entity.ToTable("Tracks");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id).HasColumnName("id");
            entity.Property(t => t.Title).HasColumnName("title");
            entity.Property(t => t.Artist).HasColumnName("artist");
            entity.Property(t => t.Image).HasColumnName("image");
            entity.Property(t => t.Genre).HasColumnName("genre");
            entity.Property(t => t.Year).HasColumnName("year");
            entity.Property(t => t.Duration).HasColumnName("duration");
            entity.Property(t => t.Isrc).HasColumnName("isrc");
            entity.Property(t => t.Email).HasColumnName("email");
            entity.Property(t => t.Country).HasColumnName("country");
            entity.Property(t => t.Lyrics).HasColumnName("lyrics");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.ToTable("Playlists");

            entity.HasKey(p => p.Id);

            entity.Property(p => p.Id).HasColumnName("id");
            entity.Property(p => p.Title).HasColumnName("title");
            entity.Property(p => p.Description).HasColumnName("description");
            entity.Property(p => p.Mood).HasColumnName("mood");
            entity.Property(p => p.Image).HasColumnName("image");
            entity.Property(p => p.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<PlaylistTrack>(entity =>
        {
            entity.ToTable("PlaylistTracks");

            entity.HasKey(pt => new { pt.PlaylistId, pt.TrackId });

            entity.Property(pt => pt.PlaylistId).HasColumnName("playlist_id");
            entity.Property(pt => pt.TrackId).HasColumnName("track_id");
            entity.Property(pt => pt.Position).HasColumnName("position").HasDefaultValue(0);

            entity.HasOne(pt => pt.Playlist)
                .WithMany(p => p.PlaylistTracks)
                .HasForeignKey(pt => pt.PlaylistId);

            entity.HasOne(pt => pt.Track)
                .WithMany(t => t.PlaylistTracks)
                .HasForeignKey(pt => pt.TrackId);
        });
    }
}