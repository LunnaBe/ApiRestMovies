using ApiRestMovies.Models;

namespace ApiRestMovies.Repositories.Interface
{
    /// <summary>
    /// Interface para o repositório de filmes, que define os métodos para acessar e manipular os dados dos filmes.
    /// </summary>
    public interface IMoviesRepository
    {
        Task<List<PlataformaMovies>> GetAllMoviesAsync();
        Task<PlataformaMovies> GetMovieByIdAsync(string id);
        Task AddMovieAsync(PlataformaMovies movie);
        Task UpdateMovieAsync(PlataformaMovies movie);
        Task DeleteMovieAsync(string id);
    }
}
