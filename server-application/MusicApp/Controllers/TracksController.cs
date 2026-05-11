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

        private static readonly List<Track> trackData = new List<Track>
        {
            new Track { Id = 1, Title = "Твоими нитями", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 2, Title = "Мне бы хотелось", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Один на один с пустотой", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Поэты", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Грузия", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Люблю", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Белый танец", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" },
            new Track { Id = 3, Title = "Новогодняя", Artist = "Зоя Ященко, Белая Гвардия · Венеция", Image = "http://localhost:5000/images/semeinoe_photo_zoya_yachenko.jpg" }

        };

        // GET: api/track/lyrics
        [HttpGet("lyrics")]
        public IActionResult GetLyrics()
        {
            var lyrics = "И когда они выходят из рамы,<br>\r\n    Улыбаясь, поправляют прически,<br>\r\n    Я для каждого в просторной прихожей<br>\r\n    Оставляю зеркала и расчески.<br>\r\n    И когда они гуляют по дому,<br>\r\n    И смеются, и играют словами,<br>\r\n    Я для них сдвигаю мягкие кресла<br>\r\n    И растапливаю печку дровами.<br><br>\r\n\r\n    А когда они листают альбомы,<br>\r\n    Удивляясь, головами качают,<br>\r\n    Я несу на блюде синие сливы,<br>\r\n    Наливаю золотистого чаю.<br>\r\n    А потом ныряю в кресло-качалку<br>\r\n    И ловлю буквально каждую фразу!<br>\r\n    Жаль, они меня не могут увидеть<br>\r\n    Даже мельком, даже краешком глаза.<br><br>\r\n\r\n    Их истории давно устарели,<br>\r\n    Платья тоже быстро вышли из моды,<br>\r\n    Но остались те же ясные лица,<br>\r\n    И во взгляде будто больше свободы.<br>\r\n    Я люблю их, вот таких старомодных,<br>\r\n    И заглядываю в зеркальце, ну же,<br>\r\n    И ищу в себе портретное сходство:<br>\r\n    Нос – ничей, зато «фамильные» уши!<br><br>\r\n\r\n    И стараюсь быть такой же свободной,<br>\r\n    Подчиняюсь только голосу крови!<br>\r\n    У меня такой же крепкий характер,<br>\r\n    И такие, как у бабушки, брови…<br>\r\n    Иногда я расчехляю гитару<br>\r\n    И наигрываю: до-ре-ми-соль-фа…<br>\r\n    И тогда они меня вспоминают,<br>\r\n    Я для них все та же девочка в гольфах.<br><br>\r\n\r\n    Между нами встало время стеною:<br>\r\n    Ни просвета, ни двери, ни окошка,<br>\r\n    Между нами будто тонкая пленка,<br>\r\n    Сквозь которую курсирует кошка.<br>\r\n    А потом упрямо близится полночь,<br>\r\n    И тогда они уходят обратно.<br>\r\n    Я свечу им фонарем на тропинку,<br>\r\n    Как же мне теперь без них, непонятно.<br><br>\r\n\r\n    Унесла их от меня электричка,<br>\r\n    Взмыла в небо желтой искоркой плавной,<br>\r\n    И запели в синих травах цикады,<br>\r\n    Это было так давно, так недавно.<br>\r\n    Протираю золоченую раму,<br>\r\n    Помоги им в долгих странствиях, Боже!<br>\r\n    Ведь когда тебе совсем одиноко,<br>\r\n    Так приятно вспоминать о хорошем."; // Это просто пример
            return Ok(new { content = lyrics });
        }

        // GET: api/track/queue
        [HttpGet("queue")]
        public IActionResult GetQueue(int page = 1, int pageSize = 3)
        {
            var pagedTracks = trackData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(new { content = pagedTracks });
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