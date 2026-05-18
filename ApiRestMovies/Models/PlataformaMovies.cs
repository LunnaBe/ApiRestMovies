using Google.Cloud.Firestore;
using System.Text.Json.Serialization;

namespace ApiRestMovies.Models
{
    public class PlataformaMovies
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("movie")]
        public string Movie { get; set; }

        [JsonPropertyName("films")]
        public List<string> Films { get; set; } = new();

        [JsonPropertyName("shortFilms")]
        public List<string> ShortFilms { get; set; } = new();

        [JsonPropertyName("tvShows")]
        public List<string> TvShows { get; set; } = new();

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("imageUrl")]
        public string ImageUrl { get; set; }
    }

    // Classe para representar a resposta da API, contendo uma mensagem e os dados dos filmes.
    public class ApiResponse
    {
        public string Mensagem { get; set; }

        public List<PlataformaMovies> Dados { get; set; }
    }
}
