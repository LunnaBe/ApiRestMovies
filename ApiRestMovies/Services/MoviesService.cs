using ApiRestMovies.Models;
using ApiRestMovies.Repositories.Interface;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

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

        private readonly IMoviesRepository _moviesRepository;

        private readonly ILogger<MoviesService> _logger;

        private readonly FirestoreDb _firestoreDb;

        private readonly HttpClient _httpClient;

        private readonly string _collectionName = "movies";


        /// <summary>
        /// Construtor da classe MoviesService, que recebe as dependências necessárias para o funcionamento do serviço, como o repositório de filmes, 
        /// o logger, a instância do FirestoreDb, o HttpClient e o nome da coleção no Firestore.
        /// </summary>
        /// <param name="moviesRepository">O repositório de filmes.</param>
        /// <param name="logger">O logger para registrar informações e erros.</param>
        /// <param name="firestoreDb">A instância do FirestoreDb para operações de banco de dados.</param>
        /// <param name="httpClient">O HttpClient para fazer requisições HTTP.</param>
        /// <param name="collectionName">O nome da coleção no Firestore.</param>
        public MoviesService(IMoviesRepository moviesRepository, ILogger<MoviesService> logger, FirestoreDb firestoreDb, HttpClient httpClient, string collectionName)
        {
            _moviesRepository = moviesRepository;
            _logger = logger;
            _firestoreDb = firestoreDb;
            _httpClient = httpClient;
            _collectionName = collectionName;
        }

        /// <summary>
        /// Listar todos os filmes disponíveis, utilizando o repositório para acessar os dados dos filmes e aplicando a lógica de negócios necessária.
        /// </summary>
        /// <returns></returns>
        public async Task<List<PlataformaMovies>> Listar()
        {
            try
            {
                // Exemplo de como acessar o Firestore para obter os filmes, caso seja necessário realizar alguma operação específica no banco de dados.
                CollectionReference collectionReference = _firestoreDb.Collection(_collectionName);

                // Log para indicar o início da obtenção dos filmes do Firestore.
                QuerySnapshot querySnapshot = await collectionReference.GetSnapshotAsync();

                // Lista de objetos do tipo PlataformaMovies, que será preenchida com os dados obtidos do Firestore.
                List<PlataformaMovies> listaMovies = new List<PlataformaMovies>();

                // Percorre os documentos obtidos do Firestore e converte cada um deles para o tipo PlataformaMovies, adicionando-os à lista de filmes.
                foreach (DocumentSnapshot document in querySnapshot.Documents)
                {
                    if (document.Exists)
                    {
                        // Converte o documento do Firestore para um objeto do tipo PlataformaMovies e adiciona à lista de filmes.
                        PlataformaMovies movie = document.ConvertTo<PlataformaMovies>();
                        listaMovies.Add(movie);
                    }
                }

                _logger.LogInformation("Iniciando a obtenção de todos os filmes.");

                return listaMovies;
            }
            catch 
            {
                _logger.LogError("Ocorreu um erro ao obter os filmes.");
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
                _logger.LogInformation($"Iniciando a obtenção do filme com ID: {id}.");

                // Exemplo de como acessar o Firestore para obter um filme específico por ID.
                DocumentReference documentReference = _firestoreDb.Collection(_collectionName).Document(id);

                // Executa a consulta para obter o documento do Firestore com o ID especificado e verifica se ele existe.
                DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

                if (documentSnapshot.Exists)
                {
                    // Converte o documento do Firestore para um objeto do tipo PlataformaMovies e retorna.
                    PlataformaMovies movie = documentSnapshot.ConvertTo<PlataformaMovies>();
                    movie.Id = documentSnapshot.Id; // Atribui o ID do documento ao objeto PlataformaMovies, caso seja necessário.
                    return movie;
                }
                else
                {
                    _logger.LogWarning($"Filme com ID: {id} não encontrado.");
                    return null; // Ou lançar uma exceção personalizada, dependendo da lógica de negócios desejada.
                }
            }
            catch
            {
                _logger.LogError($"Ocorreu um erro ao obter o filme com ID: {id}.");
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
                _logger.LogInformation("Iniciando a criação de um novo filme.");

                // Acessar o Firestore para criar um novo filme.
                CollectionReference collectionReference = _firestoreDb.Collection(_collectionName);
                await collectionReference.AddAsync(movie);

                _logger.LogInformation("Filme criado com sucesso.");
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
        /// <param name="id">ID do filme a ser atualizado.</param>
        /// <param name="movie">Objeto PlataformaMovies contendo os dados atualizados do filme.</param>
        /// <returns></returns>
        public async Task Atualizar(string id, PlataformaMovies movie)
        {
            try
            {
                _logger.LogInformation($"Iniciando a atualização do filme com ID: {id}.");

                // Acessar o Firestore para atualizar um filme específico por ID.
                DocumentReference documentReference = _firestoreDb.Collection(_collectionName).Document(id);
                await documentReference.SetAsync(movie, SetOptions.Overwrite);

                _logger.LogInformation($"Filme com ID: {id} atualizado com sucesso.");
            }
            catch
            {
                _logger.LogError($"Ocorreu um erro ao atualizar o filme com ID: {id}.");
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
                _logger.LogInformation($"Iniciando a exclusão do filme com ID: {id}.");

                // Acessar o Firestore para excluir um filme específico por ID.
                DocumentReference documentReference = _firestoreDb.Collection(_collectionName).Document(id);
                await documentReference.DeleteAsync();

                _logger.LogInformation($"Filme com ID: {id} excluído com sucesso.");
            }
            catch
            {
                _logger.LogError($"Ocorreu um erro ao excluir o filme com ID: {id}.");
                throw;
            }
        }

    }
}