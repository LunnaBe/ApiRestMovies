using ApiRestMovies.Models;
using ApiRestMovies.Repositories.Interface;
using Google.Cloud.Firestore;

namespace ApiRestMovies.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly CollectionReference _collectionReference;

        /// <summary>
        /// Construtor da classe MoviesRepository, que recebe a instância do FirestoreDb para inicializar a referência à coleção "movies" no Firestore.
        /// </summary>
        /// <param name="firestoreDb">A instância do FirestoreDb.</param>
        public MoviesRepository(FirestoreDb firestoreDb) 
        {
            // Inicializando a referência à coleção "movies" no Firestore, que será utilizada para acessar e manipular os dados dos filmes.
            _collectionReference = firestoreDb.Collection("movies");
        }

        public Task<List<PlataformaMovies>> GetAllMoviesAsync()
        {
            // Obtendo o snapshot da coleção "movies" utilizando o método GetSnapshotAsync(), que retorna um snapshot contendo os documentos (filmes) armazenados na coleção.
            var snapshot = _collectionReference.GetSnapshotAsync().Result;

            // Criando uma lista para armazenar os filmes recuperados do Firestore.
            // O método GetSnapshotAsync() retorna um snapshot da coleção, que contém os documentos (filmes) armazenados.
            var movies = new List<PlataformaMovies>();
            foreach (var document in snapshot.Documents)
            {
                var movie = document.ConvertTo<PlataformaMovies>();
                movies.Add(movie);
            }
            return Task.FromResult(movies);
        }

        /// <summary>
        /// Busca um filme específico pelo ID na coleção "movies" do Firestore. O método Document() é utilizado para acessar o documento correspondente ao ID 
        /// fornecido, e GetSnapshotAsync() é chamado para obter o snapshot do documento. 
        /// Se o documento existir, ele é convertido para um objeto do tipo PlataformaMovies utilizando o método ConvertTo<T>() e retornado. Caso contrário, retorna null.
        /// </summary>
        /// <param name="id">O ID do filme a ser buscado.</param>
        /// <returns>.</returns>
        public Task<PlataformaMovies> GetMovieByIdAsync(string id)
        {
            // Obtendo o documento do filme específico pelo ID, utilizando o método Document() para acessar o documento e GetSnapshotAsync() para obter o snapshot do documento.
            var document = _collectionReference.Document(id).GetSnapshotAsync().Result;
            if (document.Exists)
            {
                // Convertendo o snapshot do documento para um objeto do tipo PlataformaMovies utilizando o método ConvertTo<T>(),
                // que mapeia os campos do documento para as propriedades da classe PlataformaMovies.
                var movie = document.ConvertTo<PlataformaMovies>();
                return Task.FromResult(movie);
            }
            return Task.FromResult<PlataformaMovies>(null);
        }

        /// <summary>
        /// Adiciona um novo filme à coleção "movies" no Firestore. Se o ID do filme não estiver definido, um novo documento será criado com um ID gerado 
        /// automaticamente pelo Firestore. Caso contrário, o documento será criado ou atualizado com o ID especificado.
        /// </summary>
        /// <param name="movie">O objeto PlataformaMovies representando o filme a ser adicionado.</param>
        /// <returns></returns>
        public async Task AddMovieAsync(PlataformaMovies movie)
        {
            if (string.IsNullOrEmpty(movie.Id))
            {
                // Se o ID do filme não estiver definido, utilizamos o método AddAsync() para criar um novo documento com um ID gerado automaticamente pelo Firestore.
                await _collectionReference.AddAsync(movie);
            }
            else
            {
                // Se o ID do filme já estiver definido, utilizamos o método SetAsync() para criar ou atualizar o documento com o ID especificado.
                var document = _collectionReference.Document(movie.Id);
                await document.SetAsync(movie);
            }
            
        }

        /// <summary>
        /// Atualiza um filme existente na coleção "movies" do Firestore. O método Document() é utilizado para acessar o documento correspondente ao ID do filme,
        /// e SetAsync() é chamado para atualizar o documento com os novos dados do filme.
        /// </summary>
        /// <param name="movie">O objeto PlataformaMovies representando o filme a ser atualizado.</param>
        /// <returns></returns>
        public Task UpdateMovieAsync(PlataformaMovies movie)
        {
            var document = _collectionReference.Document(movie.Id);
            return document.SetAsync(movie, SetOptions.Overwrite);
        }


        /// <summary>
        /// Deleta um filme da coleção "movies" do Firestore. O método Document() é utilizado para acessar o documento correspondente ao ID do filme, 
        /// e DeleteAsync() é chamado para excluir o documento do Firestore.
        /// </summary>
        /// <param name="id">O ID do filme a ser deletado.</param>
        /// <returns></returns>
        public Task DeleteMovieAsync(string id) 
        {
            var document = _collectionReference.Document(id);
            return document.DeleteAsync();
        }
    }
}
