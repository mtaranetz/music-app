using CatalogApp.Data;
using CatalogApp.Models;   
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CatalogApp.Controllers
{
    // Контроллер для работы с треками
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ILogger<TracksController> _logger;
        private readonly ApplicationDbContext _context;

        public TracksController(
            ApplicationDbContext context,
            ILogger<TracksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}/lyrics")]
        public async Task<IActionResult> GetLyrics(int id)
        {
            var track = await _context.Tracks
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    content = t.Lyrics
                })
                .FirstOrDefaultAsync();

            if (track == null)
                return NotFound(new { message = "Трек не найден" });

            return Ok(new
            {
                content = string.IsNullOrWhiteSpace(track.content)
                    ? "Текст пока не добавлен."
                    : track.content
            });
        }

        [HttpGet("{id}/queue")]
        public async Task<IActionResult> GetQueue(int id, int pageSize = 5)
        {
            var currentTrack = await _context.Tracks.FindAsync(id);

            if (currentTrack == null)
                return NotFound(new { message = "Трек не найден" });

            var queue = await _context.Tracks
                .Where(t => t.Id != id)
                .OrderBy(t => t.Id)
                .Take(pageSize)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.Artist,
                    t.Image,
                    t.Duration
                })
                .ToListAsync();

            return Ok(new { content = queue });
        }

        // POST: api/track/addTrack
        [HttpPost("addTrack")]
        public async Task<IActionResult> AddTrack([FromBody] Track track)
        {
            if (track == null)
                return BadRequest(new { message = "Недопустимые данные" });

            if (string.IsNullOrWhiteSpace(track.Title))
                return BadRequest(new { message = "Название трека обязательно" });

            if (string.IsNullOrWhiteSpace(track.Artist))
                return BadRequest(new { message = "Артист обязателен" });

            if (string.IsNullOrWhiteSpace(track.Country))
                track.Country = "Россия";

            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Трек успешно добавлен в каталог!",
                track
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTracks(
       string? genre,
       string? year,
       string? artist,
       string? country)
        {
            var query = _context.Tracks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(t => t.Genre == genre);
            }

            if (!string.IsNullOrWhiteSpace(year))
            {
                if (year == "2020-2023")
                {
                    query = query.Where(t => t.Year.CompareTo("2020") >= 0 && t.Year.CompareTo("2023") <= 0);
                }
                else if (year == "2010-2019")
                {
                    query = query.Where(t => t.Year.CompareTo("2010") >= 0 && t.Year.CompareTo("2019") <= 0);
                }
                else if (year == "before-2010")
                {
                    query = query.Where(t => t.Year.CompareTo("2010") < 0);
                }
                else
                {
                    query = query.Where(t => t.Year == year);
                }
            }

            if (!string.IsNullOrWhiteSpace(artist))
            {
                query = query.Where(t => t.Artist.Contains(artist));
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                query = query.Where(t => t.Country == country);
            }

            var tracks = await query
                .OrderByDescending(t => t.Year)
                .ToListAsync();

            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrack(int id)
        {
            var track = await _context.Tracks.FindAsync(id);

            if (track == null)
            {
                return NotFound();
            }

            return Ok(track);
        }

        [HttpGet("artists")]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _context.Tracks
                .Where(t => !string.IsNullOrWhiteSpace(t.Artist))
                .Select(t => t.Artist)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            return Ok(artists);
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _context.Tracks
                .Where(t => !string.IsNullOrWhiteSpace(t.Country))
                .Select(t => t.Country)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return Ok(countries);
        }


    }


}