using Google.Cloud.Firestore;
using System.Text.Json;

namespace ApiRestMovies.Data
{
    public class DbMovies
    {
        // Propriedade para acessar a base de dados Firestore
        public FirestoreDb Database { get; set; }

        // Propriedade para acessar a coleção "filmes" no Firestore, que é onde os dados dos filmes serão armazenados e recuperados.
        public CollectionReference MoviesCollection => Database.Collection("filmes");

        // Construtor da classe DbMovies, que recebe a instância do FirestoreDb e a atribui à propriedade Database,
        // permitindo que as coleções sejam acessadas através das propriedades MoviesCollection e UsuariosCollection.
        public DbMovies(FirestoreDb database)
        {
            Database = database;
        }
    }
}

