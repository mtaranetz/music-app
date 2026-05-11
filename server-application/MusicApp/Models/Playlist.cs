namespace CatalogApp.Models
{
    public class Playlist
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string? Mood { get; set; }
        public string? Image { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
    }
}