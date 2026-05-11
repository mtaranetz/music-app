using CatalogApp.Data;
using CatalogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlaylistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var playlists = await _context.Playlists
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Mood,
                    p.Image,
                    p.CreatedAt,
                    TracksCount = p.PlaylistTracks.Count
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist(int id)
        {
            var playlist = await _context.Playlists
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Mood,
                    p.Image,
                    p.CreatedAt,
                    Tracks = p.PlaylistTracks
                        .OrderBy(pt => pt.Position)
                        .Select(pt => new
                        {
                            pt.Track.Id,
                            pt.Track.Title,
                            pt.Track.Artist,
                            pt.Track.Image,
                            pt.Track.Genre,
                            pt.Track.Year,
                            pt.Track.Country,
                            pt.Track.Duration
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (playlist == null)
                return NotFound(new { message = "Плейлист не найден" });

            return Ok(playlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlaylist([FromBody] Playlist playlist)
        {
            if (playlist == null || string.IsNullOrWhiteSpace(playlist.Title))
                return BadRequest(new { message = "Название плейлиста обязательно" });

            playlist.Id = 0;
            playlist.CreatedAt = DateTime.UtcNow;

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }

        [HttpPost("{playlistId}/tracks/{trackId}")]
        public async Task<IActionResult> AddTrackToPlaylist(int playlistId, int trackId)
        {
            var playlistExists = await _context.Playlists.AnyAsync(p => p.Id == playlistId);
            var trackExists = await _context.Tracks.AnyAsync(t => t.Id == trackId);

            if (!playlistExists)
                return NotFound(new { message = "Плейлист не найден" });

            if (!trackExists)
                return NotFound(new { message = "Трек не найден" });

            var alreadyExists = await _context.PlaylistTracks
                .AnyAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

            if (alreadyExists)
                return BadRequest(new { message = "Трек уже есть в плейлисте" });

            var nextPosition = await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .CountAsync();

            var playlistTrack = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = trackId,
                Position = nextPosition + 1
            };

            _context.PlaylistTracks.Add(playlistTrack);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Трек добавлен в плейлист" });
        }

        [HttpDelete("{playlistId}/tracks/{trackId}")]
        public async Task<IActionResult> RemoveTrackFromPlaylist(int playlistId, int trackId)
        {
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);

            if (playlistTrack == null)
                return NotFound(new { message = "Трек не найден в плейлисте" });

            _context.PlaylistTracks.Remove(playlistTrack);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Трек удалён из плейлиста" });
        }
    }
}