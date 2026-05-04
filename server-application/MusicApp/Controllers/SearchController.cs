using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using CatalogApp.Models;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;

        // Внедрение логера через конструктор
        public SearchController(ILogger<SearchController> logger)
        {
            _logger = logger; // Инициализация логгера
        }

 
        private static readonly List<Track> trackData = new List<Track>
        {
            new Track { Id = 1, Title = "Твоими нитями", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 2, Title = "Мне бы хотелось", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Один на один с пустотой", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" }
        };

        // GET: api/search
        [HttpGet("search")]
        public IActionResult GetSearchSuggestions(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Ok(new { suggestions = new List<string>() });
            }

            // Поиск треков, которые содержат запрос
            var suggestions = trackData
                .Where(t => t.Title.Contains(query, System.StringComparison.OrdinalIgnoreCase) || t.Artist.Contains(query, System.StringComparison.OrdinalIgnoreCase))
                .Take(5) // Ограничиваем количество подсказок
                .Select(t => t.Title) // Показываем только названия треков
                .ToList();

            // Логирование поиска
            _logger.LogInformation("Запрос на поиск: {Query}. Найдено {SuggestionsCount} подсказок", query, suggestions.Count);

            return Ok(new { suggestions });
        }
    }
}