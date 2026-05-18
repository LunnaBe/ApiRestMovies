using ApiRestMovies.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiRestMovies.Controllers
{
    [ApiController]
    [Route("")] // Deixa a rota vazia para ser a raiz do site
    public class SyncController : ControllerBase
    {
        private readonly MoviesService _moviesService;

        public SyncController(MoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        /// <summary>
        /// POST sincronizar - Sincroniza os dados da API para o Firebase.
        /// </summary>
        /// 
        /// <remarks>Sincroniza os dados da API para o Firebase.</remarks>
        /// <returns></returns>
        [HttpPost("sincronizar")]
        public async Task<IActionResult> Sincronizar()
        {
            await _moviesService.SincronizarApiParaFirebase();

            return Ok(new
            {
                mensagem = "Dados enviados para o Firebase."
            });
        }
    }
}
