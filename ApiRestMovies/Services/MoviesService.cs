using ApiRestMovies.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace ApiRestMovies.Services
{
    /// <summary>
    /// Responsável por implementar a lógica de negócios relacionada aos filmes, utilizando o repositório para acessar os dados dos filmes.
    /// </summary>
    public class MoviesService
    {
        /// <summary>
        /// Representa o repositório de filmes, que é injetado via construtor para permitir a separação de preocupações e facilitar os testes unitários. 
        /// O serviço utiliza o repositório para acessar e manipular os dados dos filmes, 
        /// aplicando a lógica de negócios necessária antes de retornar os resultados para os controladores ou outras partes da aplicação.
        /// </summary>

        private readonly ILogger<MoviesService> _logger;


        /// <summary>
        /// Construtor da classe MoviesService, que recebe as dependências necessárias para o funcionamento do serviço, como o repositório de filmes, 
        /// o logger, a instância do FirestoreDb, o HttpClient e o nome da coleção no Firestore.
        /// </summary>
        /// <param name="logger">O logger para registrar informações e erros.</param>
        public MoviesService(ILogger<MoviesService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Listar todos os filmes disponíveis, utilizando o repositório para acessar os dados dos filmes e aplicando a lógica de negócios necessária.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlataformaMovies>> Listar()
        {
            try
            {
                FirebaseClient firebase =
             new FirebaseClient(
                 "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                var firebaseMovies = await firebase
                    .Child("movies")
                    .OnceAsync<PlataformaMovies>();

                List<PlataformaMovies> movies =
                    firebaseMovies
                        .Select(item => item.Object)
                        .ToList();

                return movies;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar filmes.");
                throw;
            }
        }

        /// <summary>
        /// Obter um filme específico por ID, utilizando o repositório para acessar os dados do filme e 
        /// aplicando a lógica de negócios necessária para retornar o resultado correto.
        /// </summary>
        /// <param name="id">ID do filme a ser obtido.</param>
        /// <returns></returns>
        public async Task<PlataformaMovies> ObterPorId(string id)
        {
            try
            {
                _logger.LogInformation(
                    $"Obtendo filme com ID: {id}");

                FirebaseClient firebase =
                    new FirebaseClient(
                        "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                var movie = await firebase
                    .Child("movies")
                    .Child(id)
                    .OnceSingleAsync<PlataformaMovies>();

                if (movie == null)
                {
                    _logger.LogWarning(
                        $"Filme com ID {id} não encontrado.");

                    return null;
                }

                return movie;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Erro ao obter filme com ID: {id}");

                throw;
            }
        }


        /// <summary>
        /// Criar um novo filme, utilizando o repositório para acessar os dados dos filmes e aplicando a lógica de negócios necessária para
        /// validar e processar os dados do filme antes de criar um novo registro no banco de dados.
        /// </summary>
        /// <param name="movie">Objeto PlataformaMovies contendo os dados do filme a ser criado.</param>
        /// <returns></returns>
        public async Task Criar(PlataformaMovies movie)
        {
            try
            {
                FirebaseClient firebase =
            new FirebaseClient(
                "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                await firebase
                    .Child("movies")
                    .Child(movie.Id.ToString())
                    .PutAsync(movie);

                _logger.LogInformation(
                    "Filme salvo no Realtime Database.");
            }
            catch 
            {
                _logger.LogError("Ocorreu um erro ao criar o filme.");
                throw;
            }
        }

        /// <summary>
        /// Atualizar um filme existente, utilizando o repositório para acessar os dados dos filmes e aplicando a lógica de negócios necessária para
        /// validar e processar os dados do filme antes de atualizar o registro no banco de dados.
        /// </summary>
        /// <param name="movie">Objeto PlataformaMovies contendo os dados atualizados do filme.</param>
        /// <returns></returns>
        public async Task Atualizar(PlataformaMovies movie)
        {
            try
            {
                FirebaseClient firebase =
            new FirebaseClient(
                "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                await firebase
                    .Child("movies")
                    .Child(movie.Id.ToString())
                    .PutAsync(movie);

                _logger.LogInformation("Filme atualizado.");
            }
            catch
            {
                _logger.LogError($"Ocorreu um erro ao atualizar o filme com ID: {movie.Id}.");
                throw;
            }
        }

        /// <summary>
        /// Deletando um filme existente, utilizando o repositório para acessar os dados dos filmes e aplicando a lógica de negócios necessária para
        /// validar e processar os dados do filme antes de excluir o registro no banco de dados.
        /// </summary>
        /// <param name="id">ID do filme a ser excluído.</param>
        /// <returns></returns>
        public async Task Deletar(string id)
        {
            try
            {
                FirebaseClient firebase =
             new FirebaseClient(
                 "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                await firebase
                    .Child("movies")
                    .Child(id)
                    .DeleteAsync();

                _logger.LogInformation("Filme deletado.");
            }
            catch
            {
                _logger.LogError($"Ocorreu um erro ao excluir o filme com ID: {id}.");
                throw;
            }
        }

        /// <summary>
        /// Sincronizar os dados dos filmes da API para o Firebase, 
        /// utilizando o HttpClient para buscar os dados da API e o FirebaseClient para salvar os dados no Firebase.
        /// </summary>
        /// <returns></returns>
        public async Task SincronizarApiParaFirebase()
        {
            try
            {
                HttpClient http = new HttpClient();

                // Busca os dados da API
                var response =
                    await http.GetFromJsonAsync<ApiResponse>(
                        "http://apimoviesweb.runasp.net/api/movies");

                if (response == null || response.Dados == null)
                    return;

                FirebaseClient firebase =
                    new FirebaseClient(
                        "https://moviesfirebase-e748b-default-rtdb.firebaseio.com/");

                // Salva cada filme no Firebase
                foreach (var movie in response.Dados)
                {
                    await firebase
                        .Child("movies")
                        .Child(movie.Id.ToString())
                        .PutAsync(movie);
                }

                _logger.LogInformation(
                    "Sincronização concluída.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Erro na sincronização.");
                throw;
            }
        }

    }
}