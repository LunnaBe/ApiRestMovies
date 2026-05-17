using ApiRestMovies.Models;
using ApiRestMovies.Repositories;
using ApiRestMovies.Repositories.Interface;
using ApiRestMovies.Services;
using Google.Api.Gax.Grpc.Gcp;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Protobuf.WellKnownTypes;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// Configuração do HttpClient para fazer requisições HTTP, que pode ser utilizado pelos serviços para acessar APIs externas ou realizar chamadas HTTP conforme necessário.
builder.Services.AddHttpClient();

// Swagger + documentação XML
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API REST de Filmes",
        Description = "Uma API REST para gerenciar filmes, utilizando Firestore como banco de dados.",
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

// Configuração das credencias do Firebase
var caminhoCredencialFirabase = Path.Combine(Directory.GetCurrentDirectory(), "moviesfirebase.json");
var credential = GoogleCredential.FromFile(caminhoCredencialFirabase);

// Configuração do FirestoreDbBuilder para criar uma instância do FirestoreDb, utilizando as
// configurações definidas no arquivo de configuração da aplicação (appsettings.json).
var firestoreDb = new FirestoreDbBuilder
{
    // As configurações de ProjectId e CredentialsPath são lidas do arquivo de configuração da aplicação (appsettings.json) para garantir que
    // as credenciais e o ID do projeto sejam configurados corretamente.
    ProjectId = builder.Configuration["ApiConfig:ProjectId"],
    Credential = credential
}.Build();

// Adiciona o FirestoreDb como um serviço singleton no contêiner de injeção de dependência, garantindo que a mesma instância seja utilizada em toda a aplicação.
builder.Services.AddSingleton(firestoreDb);

// Configuração de injeção de dependência para os serviços e repositórios, permitindo que as dependências
builder.Services.AddSingleton<PlataformaMovies>();
builder.Services.AddScoped<MoviesService>();

// Configuração de injeção de dependência para o FirestoreDb, permitindo que ele seja utilizado pelos repositórios e serviços que precisam acessar o banco de dados.
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


var app = builder.Build();

// url personalizada para acessar a documentação - http://localhost:5000/documentacao
app.UseSwagger();
app.UseSwaggerUI();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "documentacao";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Filmes v1");
});

// Configuração do middleware de CORS para permitir solicitações de qualquer origem, o que é útil durante o desenvolvimento e testes,
// mas deve ser configurado com mais restrição em ambientes de produção para garantir a segurança da aplicação.
app.UseCors("PermitirTudo");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
