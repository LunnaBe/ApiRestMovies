using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Builder.Extensions;
using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Firebase.Database;
using Google.Cloud.Firestore.V1;

namespace ApiRestMovies.Data
{
    public class DbMovies
    {
        // Propriedade para acessar a base de dados Firestore
        public FirestoreDb Database { get; }

        // Propriedade para acessar a coleção "filmes" no Firestore, que é onde os dados dos filmes serão armazenados e recuperados.
        public CollectionReference MoviesCollection => Database.Collection("filmes");

        // Instância única do cliente (padrão Singleton)
        public FirebaseClient RealtimeDb { get; }

        // Construtor da classe DbMovies, que recebe a instância do FirestoreDb e a atribui à propriedade Database,
        // permitindo que as coleções sejam acessadas através das propriedades MoviesCollection e UsuariosCollection.
        public DbMovies(IConfiguration configuration)
        {
            // Inicializa a conexão com o Firestore usando as credenciais do arquivo JSON
            var ProjectId = configuration["Firebase:ProjectId"];
            var Credential = configuration["Firebase:CredentialFilePath"];
            var credentialJson = configuration["Firebase:CredentialFilePath"];

            // Verificação se as configurações do Firebase estão completas, lançando uma exceção caso alguma configuração esteja faltando ou seja inválida.
            if (string.IsNullOrEmpty(ProjectId) || string.IsNullOrEmpty(Credential))
            {
                throw new ArgumentException("As configurações do Firebase estão incompletas. Verifique o arquivo de configuração.");
            }

            // Inicializa a autenticação com o arquivo de chave de serviço, criando uma instância do FirebaseApp se ainda não
            // existir, utilizando as credenciais fornecidas no arquivo JSON.
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credentialJson)
                });
            }

            // Define a variável de ambiente "GOOGLE_APPLICATION_CREDENTIALS" com o caminho para o arquivo de credenciais do Firebase,
            // garantindo que as credenciais sejam corretamente configuradas para autenticação com o Firestore.
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialJson);

            // Inicializa a conexão com o Firestore usando as credenciais do arquivo JSON e o ID do projeto,
            // criando uma instância do FirestoreDb que será utilizada para acessar a base de dados Firestore.
            Database = FirestoreDb.Create(ProjectId);

            // Inicializa a conexão com o Realtime Database do Firebase, utilizando a URL
            // do banco de dados e uma função assíncrona para obter o token de autenticação personalizado.
            RealtimeDb = new FirebaseClient(configuration["Firebase:RealtimeDatabaseUrl"], new FirebaseOptions
            {
                AuthTokenAsyncFactory = async () =>
                {
                    // Gerar um token de autenticação personalizado para o usuário "admin" utilizando o FirebaseAuth.
                    var token = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync("admin");
                    return token;
                }
            });

        }

    }
}

