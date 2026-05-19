using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;

namespace ApiRestMovies.Data
{
    public class DbMovies
    {
        // Propriedade que representa a instância do FirebaseClient para acessar o Realtime Database do Firebase.
        public FirebaseClient RealtimeDb { get; }

        // Construtor da classe DbMovies, que recebe a configuração do aplicativo para acessar
        // as credenciais do Firebase e configurar a conexão com o Realtime Database.
        [Obsolete]
        public DbMovies(IConfiguration configuration)
        {
            var credentialJson =
                configuration["Firebase:CredentialFilePath"];

            var pastaExecucao =
                AppContext.BaseDirectory;

            var caminhoCompletoChave =
                Path.Combine(
                    pastaExecucao,
                    credentialJson);

            // Verifica se a instância do FirebaseApp já foi criada para evitar criar múltiplas instâncias, o que pode causar erros.
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential =
                        GoogleCredential.FromFile(
                            caminhoCompletoChave)
                });
            }
            // Configura a instância do FirebaseClient para acessar o Realtime Database,
            // utilizando as credenciais do Firebase para autenticação.
            RealtimeDb = new FirebaseClient(
                configuration["Firebase:RealtimeDatabaseUrl"],
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = async () =>
                    {
                        var token =
                            await FirebaseAuth
                                .DefaultInstance
                                .CreateCustomTokenAsync("admin");

                        return token;
                    }
                });
        }
    }
}