using System.Text.Json.Serialization;

namespace MovieCatalogAPI.Models
{
    public class Director
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        [JsonIgnore]
        public List<Movie>? Movies { get; set; }
    }
}
