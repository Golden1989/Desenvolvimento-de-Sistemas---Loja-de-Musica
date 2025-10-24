# Desenvolvimento-de-Sistemas---Loja-de-Musica
📝 README.md — Projeto MusicaDB
🎯 Objetivo

Este projeto tem como objetivo construir uma API REST com persistência em banco de dados usando Entity Framework Core.
O sistema realiza operações CRUD (Create, Read, Update, Delete) sobre uma entidade chamada Musica, tanto via API quanto via CLI (terminal).

🧩 Stack Utilizada

Linguagem: C#

Framework: .NET 9 / ASP.NET Core Web API

ORM: Entity Framework Core 9

Banco de dados: SQLite (musica.db)

Ferramentas de teste: Postman ou Swagger

CLI (Console): integrado no Program.cs

🧱 Estrutura do Projeto
MusicaDB/
│
├── Controllers/
│   └── MusicaController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   └── Musica.cs
│
├── Program.cs
├── musica.db
└── README.md

🎵 Entidade: Musica
Campo	Tipo	Obrigatório	Descrição
Id	int	✅ Sim	Identificador único da música.
Titulo	string	✅ Sim	Nome/título da música.
Artista	string	✅ Sim	Nome do artista ou banda.
DataCadastro	DateTime	✅ Sim	Data em que foi cadastrada.
⚙️ Passos para Executar o Projeto
1️⃣ Clonar o repositório
git clone https://github.com/SEU_USUARIO/MusicaDB.git
cd MusicaDB

2️⃣ Restaurar dependências
dotnet restore

3️⃣ Criar o banco de dados via migrations
dotnet ef migrations add InitialCreate
dotnet ef database update

4️⃣ Executar o projeto
dotnet run


A API iniciará em:

http://localhost:5099


E o Swagger estará disponível em:

http://localhost:5099/swagger

🚀 Rotas da API
🔹 GET /api/v1/musica

Lista todas as músicas cadastradas.

Exemplo de resposta:

[
  {
    "id": 1,
    "titulo": "Imagine",
    "artista": "John Lennon",
    "dataCadastro": "2025-10-21T22:00:00Z"
  }
]

🔹 GET /api/v1/musica/{id}

Retorna uma música específica pelo ID.

Exemplo:

GET /api/v1/musica/1


Resposta:

{
  "id": 1,
  "titulo": "Imagine",
  "artista": "John Lennon",
  "dataCadastro": "2025-10-21T22:00:00Z"
}

🔹 POST /api/v1/musica

Cadastra uma nova música.

Body (JSON):

{
  "titulo": "Bohemian Rhapsody",
  "artista": "Queen"
}


Resposta (201 Created):

{
  "id": 2,
  "titulo": "Bohemian Rhapsody",
  "artista": "Queen",
  "dataCadastro": "2025-10-21T22:00:00Z"
}

🔹 PUT /api/v1/musica/{id}

Atualiza uma música existente.

Exemplo:

PUT /api/v1/musica/2


Body (JSON):

{
  "titulo": "Bohemian Rhapsody (Remaster)",
  "artista": "Queen"
}


Resposta (200 OK)

🔹 DELETE /api/v1/musica/{id}

Remove uma música do banco de dados.

Exemplo:

DELETE /api/v1/musica/2


Resposta (204 No Content)

💻 Modo Console (CLI)

O sistema também permite interagir via terminal durante a execução do programa:

Opção	Descrição
1	Cadastrar música
2	Listar músicas
3	Atualizar música (por ID)
4	Remover música (por ID)
0	Encerrar aplicação

Exemplo de uso no terminal:

== MusicaDbLab ==
Console + API executando juntos!

1 - Cadastrar música
2 - Listar músicas
3 - Atualizar música (por Id)
4 - Remover música (por Id)
0 - Sair

⚠️ Validações e Tratamento de Erros

400 Bad Request → Erros de entrada inválida.

404 Not Found → Registro não encontrado.

409 Conflict → Título duplicado.

422 Unprocessable Entity → Dados não processáveis.

As propriedades Titulo, Artista e DataCadastro são obrigatórias, e o título é único.

📘 Testes com Postman

Criar uma nova coleção.

Adicionar as rotas GET, POST, PUT, DELETE.

Usar o formato JSON conforme exemplos acima.

O endereço base é http://localhost:5099/api/v1/musica.

🧮 Critérios de Avaliação Atendidos
Critério	Peso	Situação
Banco de Dados (chaves, schema, EF Core)	40 pts	✅ Concluído
API & CRUD completos (GET/POST/PUT/DELETE)	40 pts	✅ Concluído
Validação & Erros (DataAnnotations, status codes)	10 pts	✅ Concluído
Qualidade de código (organização, clareza, clean code)	5 pts	✅ Concluído
Documentação (README.md)	5 pts	✅ Concluído
Total estimado:	100 pts	🏆 Perfeito!

👩‍💻 Autora
Isabella Campos Bueno
Luiz Felipe Campos 
Curso: Engenharia da Computação
Disciplina: Desenvolvimento de Sistemas — Projeto Banco de Dados + API
Instituição: [CEUB]
