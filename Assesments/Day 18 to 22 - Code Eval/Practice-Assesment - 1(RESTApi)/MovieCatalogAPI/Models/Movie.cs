using System.Text.Json.Serialization;

namespace MovieCatalogAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public int DirectorId { get; set; }
        [JsonIgnore]
        public Director? Director { get; set; }
    }
}
