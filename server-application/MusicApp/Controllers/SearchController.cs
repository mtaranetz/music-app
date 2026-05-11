using CatalogApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new { suggestions = new List<object>() });

            var suggestions = await _context.Tracks
                .Where(t =>
                    t.Title.Contains(query) ||
                    t.Artist.Contains(query))
                .Take(5)
                .Select(t => new
                {
                    id = t.Id,
                    title = t.Title,
                    artist = t.Artist,
                    image = t.Image,
                    url = $"/pages/track_cards/track-card_01.html?track={t.Id}"
                })
                .ToListAsync();

            return Ok(new { suggestions });
        }
    }
}