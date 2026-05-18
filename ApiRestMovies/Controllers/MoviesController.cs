using ApiRestMovies.Data;
using ApiRestMovies.Models;
using ApiRestMovies.Services;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
namespace ApiRestMovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class MoviesController : ControllerBase
    {
        private readonly MoviesService _moviesService;
        private readonly DbMovies _db;



        /// <summary>
        /// Construtor da classe MoviesController, que recebe a dependência do MoviesService para permitir a separação de preocupações e facilitar os testes unitários.
        /// </summary>
        /// <param name="moviesService">A instância do MoviesService.</param>
        /// <param name="db">A instância do DbMovies.</param>
        public MoviesController(MoviesService moviesService, DbMovies db)
        {
            _moviesService = moviesService;
            _db = db;
        }


        /// <summary>
        /// GET api/movies/firestore - Obtém a lista de filmes diretamente do Firestore.
        /// </summary>
        /// <remarks>Retorna a lista de filmes armazenados no Firestore.</remarks>
        /// <returns></returns>
        [HttpGet("firestore")]
        public async Task<IActionResult> GetFromFirestore()
        {
            try
            {
                var snapshot = await _db.MoviesCollection.GetSnapshotAsync();
                var filmes = snapshot.Documents.Select(d => d.ToDictionary()).ToList();
                return Ok(filmes);
            }
            catch 
            {
                // Log para indicar que ocorreu um erro ao obter os filmes do Firestore.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Firestore");
            }
        }

        /// <summary>
        /// GET api/movies/realtime - Obtém a lista de filmes diretamente do Realtime Database do Firebase.
        /// </summary>
        /// 
        /// <remarks>Retorna a lista de filmes armazenados no Realtime Database do Firebase.</remarks>
        /// <returns></returns>
        [HttpGet("realtime")]
        public async Task<IActionResult> GetFromRealtime()
        {
            try
            {
                // Busca um nó chamado "filmes" na árvore JSON do Realtime
                var filmes = await _db.RealtimeDb
                    .Child("filmes").OnceAsync<object>();

                return Ok(filmes);
            }
            catch
            {
                // Log para indicar que ocorreu um erro ao obter os filmes do Realtime Database.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro no Realtime");
            }
        }

        /// <summary>
        /// GET api/movies - Obtém a lista de todos os filmes disponíveis.
        /// </summary>
        /// 
        /// <remarks>Retorna a lista de filmes disponíveis.</remarks>
        /// 
        /// <response code="200">Retorna a lista de filmes.</response>
        /// <response code="500">Ocorreu um erro ao processar a solicitação.</response> 
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMovies()
        {
            try
            {
                var movies = await _moviesService.Listar();
                return Ok(new { mensagem = "Filmes obtidos com sucesso", dados = movies });
            }
            catch  
            {
                // Log para indicar que ocorreu um erro ao obter os filmes.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter os filmes");
            }
        }

        /// <summary>
        /// GET api/movies/{id} - Obtém um filme específico pelo seu ID.
        /// </summary>
        /// 
        /// <remarks>Retorna um filme específico pelo seu ID.</remarks>
        /// 
        /// <param name="id">O ID do filme.</param>
        /// <response code="200">Retorna o filme encontrado.</response>
        /// <response code="404">O filme com o ID especificado não foi encontrado.</response>
        /// <response code="500">Ocorreu um erro ao processar a solicitação.</response>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovieById(string id)
        {
            try
            {
                var movie = await _moviesService.ObterPorId(id);
                if (movie == null)
                {
                    return NotFound(new { mensagem = $"Filme com id {id} não encontrado" });
                }

                return Ok(new { mensagem = "Filme obtido com sucesso", dados = movie });
            }
            catch 
            {
                // Log para indicar que ocorreu um erro ao obter o filme.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao obter o filme");
            }
        }

        /// <summary>
        /// POST api/movies - Adiciona um novo filme à coleção.
        /// </summary>
        /// 
        /// <remarks>Adiciona um novo filme à coleção.</remarks>
        /// 
        /// <response code="201">Filme adicionado com sucesso.</response>
        /// <response code="400">Dados do filme são inválidos.</response>
        /// <response code="500">Ocorreu um erro ao adicionar o filme.</response>
        /// <param name="movie">O filme a ser adicionado.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AddMovie([FromBody] PlataformaMovies movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movie == null)
            {
                return BadRequest(new { mensagem = "Dados do filme são inválidos." });
            }

            try
            {
                await _moviesService.Criar(movie);

                // Retorna um status 201 Created com a localização do novo recurso criado e os dados do filme adicionado.
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, new { mensagem = "Filme adicionado com sucesso", dados = movie });
            }
            catch 
            {
                // Log para indicar que ocorreu um erro ao adicionar o filme.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao adicionar o filme");
            }

        }

        /// <summary>
        /// PUT api/movies/{id} - Atualiza um filme existente pelo seu ID.
        /// </summary>
        /// 
        /// <remarks>Atualiza um filme existente pelo seu ID.</remarks>
        /// <param name="id">ID do filme a ser atualizado.</param>
        /// <param name="movie">Objeto PlataformaMovies contendo os dados atualizados do filme.</param>
        /// 
        /// <response code="200">Retorna o filme atualizado com sucesso.</response>
        /// <response code="400">Dados do filme são inválidos ou o ID no corpo da requisição não corresponde ao ID na URL.</response>
        /// <response code="404">Filme com o ID especificado não encontrado.</response>
        /// <response code="500">Ocorreu um erro ao atualizar o filme.</response>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMovie(string id, [FromBody] PlataformaMovies movie)
        {
            try
            {
                // Verifica se o filme é nulo ou se o ID do filme no corpo da requisição não corresponde ao ID na URL. Se for o caso, retorna um status 400 Bad Request.
                if (movie == null || id != movie.Id)
                {
                    return BadRequest(new { mensagem = "Dados do filme são inválidos" });
                }
                var existingMovie = await _moviesService.ObterPorId(id);
                if (existingMovie == null)
                {
                    return NotFound(new { mensagem = $"Filme com id {id} não encontrado" });
                }
                await _moviesService.Atualizar(movie);
                return Ok(new { mensagem = "Filme atualizado com sucesso", dados = movie });
            }
            catch 
            {
                // Log para indicar que ocorreu um erro ao atualizar o filme.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar o filme");
            }
        }

        /// <summary>
        /// DELETE api/movies/{id} - Deleta um filme existente pelo seu ID.
        /// </summary>
        /// 
        /// <remarks>Deleta um filme existente pelo seu ID.</remarks>
        /// 
        /// <response code="200">Filme deletado com sucesso.</response>
        /// <response code="404">Filme com o ID especificado não encontrado.</response>
        /// <response code="500">Ocorreu um erro ao deletar o filme.</response>
        /// <param name="id">ID do filme a ser deletado.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            try
            {
                var existingMovie = await _moviesService.ObterPorId(id);
                if (existingMovie == null)
                {
                    return NotFound(new { mensagem = $"Filme com id {id} não encontrado" });
                }
                await _moviesService.Deletar(id);
                return Ok(new { mensagem = "Filme deletado com sucesso" });
            }
            catch 
            {
                // Log para indicar que ocorreu um erro ao deletar o filme.
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar o filme");
            }
        }
    }
}