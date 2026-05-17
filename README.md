# ApiRestMovies __
## API REST de gerenciamento de filmes.

Web API REST moderna, cujo objetivo principal é funcionar como um Catálogo, Agregador ou Gerenciador de Plataformas de Streaming e Conteúdos Digitais (como filmes, curtas-metragens e programas de TV). Utilizando o Firebase (Firestore e Realtime Database) para persistência de dados NoSQL escalável e em tempo real. A aplicação conta com documentação viva e interativa via Swagger/OpenAPI, facilitando a integração com qualquer aplicação client.

---

### Arquitetura
- **ASP.NET Core Web API** - Framework principal para desenvolvimento de API
- **SDK .NET 8.0** - Plataforma principal para o projeto API
- **Visual Studio 2022 ou JetBrains Rider** - IDE(Ambiente de Desenvolvimento Integrado) para criação da API
- **C#(C-Sharp)** - Linguagem utilizada na API
- **Swagger/OpenAPI** - Documentação automática da API
- **Injeção de Dependências** - Padrão de injeção de dependências do .NET

---

### Gerenciador de Pacotes NuGet
- **FirebaseAdmin (v3.5.0)** - Gerencia a segurança e autenticação da API com os serviços do Google Cloud através de chaves privadas.
- **FirebaseDatabase.net (v5.0.0)** - Permite a comunicação e sincronização de dados em tempo real com o Firebase Realtime Database.
- **Google.Cloud.Firestore (v4.2.0)** - Banco de dados NoSQL principal (baseado em documentos) usado para salvar e consultar o catálogo de plataformas e filmes na nuvem.
- **Swashbuckle.AspNetCore (v6.6.2)** - (Swagger): Gera automaticamente a documentação interativa da API, permitindo testar os endpoints direto pelo navegador.

---

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











