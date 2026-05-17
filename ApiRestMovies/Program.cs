using ApiRestMovies.Data;
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

// Configuração do Swagger para gerar a documentação da API, incluindo informações como versão, título e descrição da API.
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

    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

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

// Configuração de injeção de dependência para a classe DbMovies, que é responsável por gerenciar a conexão com o Firestore e fornecer acesso às coleções do banco de dados.
builder.Services.AddScoped<DbMovies>();

// Configuração de injeção de dependência para os repositórios e serviços, permitindo que as dependências sejam resolvidas automaticamente pelo contêiner de injeção de dependência.
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<MoviesService>();


var app = builder.Build();

// url personalizada para acessar a documentação - http://localhost:7123/documentacao
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

// Rotas para a API
app.MapControllers();
app.MapGet("/", () => "API REST Movies Online");

app.Run();
