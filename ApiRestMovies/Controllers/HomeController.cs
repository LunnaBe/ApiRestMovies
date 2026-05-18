using Microsoft.AspNetCore.Mvc;

namespace ApiRestMovies.Controllers
{
    [ApiController]
    [Route("")] // Deixa a rota vazia para ser a raiz do site
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                alerta = "Conexão estabelecida com sucesso!",
                projeto = "API REST Movies",
                horarioServidor = DateTime.Now
            });
        }
    }
}
