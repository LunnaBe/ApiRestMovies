using Google.Cloud.Firestore;
using System.Text.Json;

namespace ApiRestMovies.Data
{
    public class DbMovies
    {
        // Propriedade para acessar a base de dados Firestore
        public FirestoreDb Database { get; set; }

        public DbMovies()
        {
            // Caminho para o arquivo de credenciais JSON
            var caminhoChaveWeb = "config_API/firebase-key.json";
            var folders = Path.Combine(AppContext.BaseDirectory, caminhoChaveWeb);

            // Verificando se o arquivo de credenciais existe
            if (!File.Exists(folders))
            {
                throw new FileNotFoundException($"ERRO CRÍTICO: Arquivo não encontrado em: {folders}");
            }

            // Lendo o conteúdo do arquivo JSON para extrair o movies_id
            string jsonString = File.ReadAllText(folders);

            // Analisando o JSON para obter o movies_id
            using JsonDocument doc = JsonDocument.Parse(jsonString);

            // Verificando se a propriedade "movies_id" existe no JSON
            if (!doc.RootElement.TryGetProperty("movies_id", out var moviesIdElement))
            {
                throw new Exception("O arquivo JSON é inválido. A propriedade 'movies_id' não foi encontrada.");
            }

            // Extraindo o valor do movies_id
            string moviesId = moviesIdElement.GetString()!;

            // MODO DEFINITIVO: Injeção direta e explícita da credencial física
            var builder = new FirestoreDbBuilder
            {
                MoviesId = moviesId,
                CredentialsPath = folders
            };

            Database = builder.Build();
        }
    }
}


    }
}
