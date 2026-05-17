# ApiRestMovies __
## API REST de gerenciamento de filmes.


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
- **ASP.NET Core 8.0** - Framework principal para desenvolvimento de API
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
