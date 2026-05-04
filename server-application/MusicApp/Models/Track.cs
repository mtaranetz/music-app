namespace CatalogApp.Models
{
    public class Track
    {
        public int Id { get; set; }            // Идентификатор трека
        public string Title { get; set; }      // Название трека
        public string Artist { get; set; }     // Исполнитель
        public string Image { get; set; }      // Обложка
        public string Genre { get; set; }      // Жанр
        public string Year { get; set; }          // Год выпуска
        public string Country { get; set; } = string.Empty; // Страна

        public string Duration { get; set; }   // Длительность
        public string Isrc { get; set; }       // ISRC код
        public string Email { get; set; }      // Email правообладателя
    }
}