namespace ApiRestMovies.Models
{
    // Classe para representar a resposta da API, contendo uma mensagem e os dados dos filmes.
    public class ApiResponse
    {
        public string Mensagem { get; set; }

        public List<PlataformaMovies> Dados { get; set; }
    }
}
