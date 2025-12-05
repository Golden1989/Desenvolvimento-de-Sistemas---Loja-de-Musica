#Desenvolvimento-de-sistemas---Loja-de-Musica
📌 📁 Estrutura do Projeto
/MusicaDb
│── MusicaDb.API        → API REST (ASP.NET 8) com CRUD de Músicas e Álbuns
│── MusicaDb.GUI        → Aplicação WPF integrada com a API
│── MusicaDb.sln        → Solução principal

🚀 1. Como rodar o projeto completo
✔ Passo 1 — Rodar a API

No terminal:

cd MusicaDb/MusicaDb.API
dotnet run


A API vai iniciar em:

http://localhost:5099

Endpoints incluem:

GET /api/v1/musica

GET /api/v1/musica/{id}

POST /api/v1/musica

PUT /api/v1/musica/{id}

DELETE /api/v1/musica/{id}

GET /api/v1/album

✔ Passo 2 — Rodar a Interface WPF

Em outro terminal:

cd MusicaDb/MusicaDb.GUI
dotnet run


A GUI abrirá automaticamente e se conectará à sua API.

🧠 2. Como a GUI funciona

A aplicação WPF usa a classe ApiClient para consumir os endpoints:

public class ApiClient
{
    private readonly HttpClient _http = new()
    {
        BaseAddress = new Uri("http://localhost:5099/api/v1/")
    };
}


Todos os comandos CRUD da interface chamam sua API real.

📌 Tela Principal – Listagem de Músicas

✔ Lista todas as músicas
✔ Permite filtrar pelo título
✔ Mostra artista, gênero e nome do álbum
✔ Botões CRUD

🔍 Busca de Músicas

Digite um texto e clique Buscar.

A busca é feita localmente na lista carregada da API.

➕ Adicionar Música

Botão Adicionar abre esta janela:

Título:

Artista:

Gênero:

Seleção de Álbum carregado da API:

Ao salvar → chama o endpoint:

POST /api/v1/musica

✏ Editar Música

Ao selecionar uma música → clique em Editar.
O formulário abre preenchido.

Ao salvar → chama:

PUT /api/v1/musica/{id}

🗑 Excluir Música

Ao clicar em Excluir:

Confirmação (MessageBox)

Chama:

DELETE /api/v1/musica/{id}


🧩 4. Estrutura da GUI WPF
MusicaDb.GUI
│── MainWindow.xaml              → Tela principal (listagem + pesquisa + CRUD)
│── MainWindow.xaml.cs           → Lógica da tela principal
│── Views/
│     └── EditarMusicaWindow.xaml → Janela de criação/edição
│── Services/
│     └── ApiClient.cs           → Comunicação com a API
│── Models/
      ├── MusicaDTO.cs
      ├── AlbumDTO.cs
      └── MusicaCreateDTO.cs

🔌 5. Comunicação com a API

A GUI usa HttpClient para enviar e receber JSON:

✔ Listar músicas
_http.GetFromJsonAsync<List<MusicaDTO>>("musica");

✔ Criar música
_http.PostAsJsonAsync("musica", dto);

✔ Atualizar
_http.PutAsJsonAsync($"musica/{id}", dto);

✔ Excluir
_http.DeleteAsync($"musica/{id}");

🧱 6. Como testar no Postman

Exemplos de requisições:

📌 GET – Listar músicas
GET http://localhost:5073/api/v1/musica

📌 POST – Criar música
POST http://localhost:5073/api/v1/musica
{
  "titulo": "Minha Música",
  "artista": "Fulano",
  "genero": "Rock",
  "albumId": 1
}

📌 PUT – Atualizar
PUT http://localhost:5073/api/v1/musica/1

📌 DELETE – Remover
DELETE http://localhost:5073/api/v1/musica/1

🛠 7. Requisitos

.NET 8 ou superior

Windows (para WPF)

API rodando antes da GUI

Postman (opcional)

📦 8. Como clonar e rodar
git clone https://github.com/SEU_USUARIO/Desenvolvimento-de-Sistemas---Loja-de-Musica.git
cd Desenvolvimento-de-Sistemas---Loja-de-Musica

API:
cd MusicaDb/MusicaDb.API
dotnet run

GUI:
cd ../MusicaDb.GUI
dotnet run

🎓 9. Objetivo Acadêmico

Este projeto demonstra:

✔ Criação de API REST
✔ Consumo de API por GUI WPF
✔ Padrão DTO
✔ CRUD completo
✔ Comunicação JSON
✔ Uso de HttpClient
✔ Separação entre backend e frontend desktop

🧑‍💻 10. Autora

Isabella Campos Bueno
Luiz Felipe Campos da Silva
Desenvolvedora • Engenharia da Computação • Cybersecurity
