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
---

### Dificuldades no Técnicas
#### 1. O Conflito de Rotas no Servidor (Erro 500 / 404)
- **Dificuldade:** Logo no início, a aplicação funcionava no seu computador, mas ao subir para a nuvem, ela dava erro 500 ou não encontrava a página. Isso acontecia porque o arquivo Program.cs tinha comandos duplicados para o Swagger (app.UseSwaggerUI()), o que confundia o servidor IIS da hospedagem.

- **Resolução:** Limpamos o código do Program.cs, removemos as duplicidades e isolamos a rota do Swagger mudando o nome de /swagger para /documentacao. Isso deu um "caminho exclusivo" para a sua página de testes rodar sem travar.

#### 2. A Injeção de Dependência Esquecida (Erro 500 no Banco)
- **Dificuldade:** Em um dos momentos, o código compilava, mas ao tentar listar ou salvar os filmes, a API quebrava internamente. Descobrimos que as linhas que conectavam a Controller ao banco de dados Firestore (DbMovies e FirestoreDb) tinham sumido do Program.cs. A Controller pedia o banco, mas o projeto não sabia onde ele estava.

- **Resolução:** Adicionamos novamente os serviços do Firestore (AddScoped<DbMovies>) logo após a configuração da sua chave de segurança "moviesFirebaseKey.json", fazendo a ponte voltar a funcionar.

#### 3. Arquivos Travados na Memória da Hospedagem (Permission Denied / Erro 503)
- **Dificuldade:** Quando você tentava atualizar a API na nuvem (MonsterASP), o painel mostrava um erro vermelho dizendo que você não tinha permissão para deletar a ApiRestMovies.dll. Logo em seguida, o site ficava "OFFLINE" com erro 503. Isso acontecia porque a API continuava ligada na internet, e o servidor "segurava" a DLL na memória para os usuários usarem, impedindo você de substituí-la.

- **Resolução:** Entendemos o ciclo correto de deploy em servidores IIS: primeiro acessamos o painel e clicamos em STOP (para o servidor soltar a DLL), apagamos a pasta antiga, subimos os arquivos novos gerados pelo Visual Studio e só então clicamos em START.

#### 4. Cache do Navegador Local (Erro 404 no Localhost)
- **Dificuldade:** Depois que consertamos o código e você foi testar no seu computador, o navegador tentava abrir localhost:.../swagger e dava erro 404. O navegador estava acostumado com o endereço antigo e não sabia que tínhamos mudado a rota.

- **Resolução:** Você aprendeu a acessar a URL correta (/documentacao) e nós fomos no arquivo de configuração interna do Visual Studio (launchSettings.json) e alteramos o launchUrl para documentacao. Agora, sempre que você aperta o botão de "Play", o projeto já abre sabendo para onde ir.











