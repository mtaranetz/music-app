namespace CatalogApp.Models
{
    public class Track
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Artist { get; set; } = "";
        public string Image { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Year { get; set; } = "";
        public string Country { get; set; } = "";
        public string Duration { get; set; } = "";
        public string Isrc { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Lyrics { get; set; }

        public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
    }
}