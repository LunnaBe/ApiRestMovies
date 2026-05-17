# ApiRestMovies __
## API REST de gerenciamento de filmes.

Web API REST moderna, cujo objetivo principal é funcionar como um Catálogo, Agregador ou Gerenciador de Plataformas de Streaming e Conteúdos Digitais (como filmes, curtas-metragens e programas de TV). Utilizando o Firebase (Firestore e Realtime Database) para persistência de dados NoSQL escalável e em tempo real. A aplicação conta com documentação viva e interativa via Swagger/OpenAPI, facilitando a integração com qualquer aplicação client.

### Estrutura do Projeto

```
📂 ApiRestMovies
├── 📂 Controllers
│   └── MoviesController.cs
├── 📂 Data
│   └── DbMovies.cs
├── 📂 Models
│   └── PlataformaMovies.cs
├── 📂 Repositories
│   ├── 📂 Interface
│       └── IMoviesRepository.cs
│   └── MoviesRepository.cs
├── 📂 Services
│   └── MoviesService.cs
├── ApiRestMovies.http
├── appsettings.json
│   └── appsettings.Development.json
└── Program.cs
```
---

### Arquitetura
- **ASP.NET Core Web API** - Framework principal para desenvolvimento de API
- **.NET 8.0** - Plataforma principal para o projeto API
- **Visual Studio 2022** - IDE(Ambiente de Desenvolvimento Integrado) para criação da API
- **C#(C-Sharp)** - Linguagem utilizada na API
- **Swagger/OpenAPI** - Documentação automática da API
- **Injeção de Dependências** - Padrão de injeção de dependências do .NET

---

### Recursos Funcionais
#### CRUD
- **GET api/movies/firestore** - Obtém a lista de filmes diretamente do Firestore.
- **GET api/movies/realtime** - Obtém a lista de filmes diretamente do Realtime Database do Firebase.
- **GET api/movies** - Obtém a lista de todos os filmes disponíveis.
- **GET api/movies/{id}** - Obtém um filme específico pelo seu ID.
- **POST api/movies** - Adiciona um novo filme à coleção.
- **PUT api/movies/{id}** - Atualiza um filme existente pelo seu ID.
- **DELETE api/movies/{id}** - Deleta um filme existente pelo seu ID.

---

### Estrutura de Dados
#### Dados - PlataformaMovies
``` csharp
public class PlataformaMovies
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Films { get; set; }
    public string ShortFilms { get; set; }
    public string TvShows { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
}
```











