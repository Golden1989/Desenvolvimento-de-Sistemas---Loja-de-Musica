using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using musica.Data;
using musica.Models;

var builder = WebApplication.CreateBuilder(args);

// Porta fixa (igual ao exemplo do professor)
builder.WebHost.UseUrls("http://localhost:5099");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=musica.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Executa migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

var webTask = app.RunAsync();
Console.WriteLine("API online em http://localhost:5099 (Swagger em /swagger)");

Console.WriteLine("== MusicStoreDb ==");
Console.WriteLine("Console + API executando juntos!\n");

while (true)
{
    Console.WriteLine("\nEscolha uma opção:");
    Console.WriteLine("1 - Cadastrar Álbum");
    Console.WriteLine("2 - Listar Álbums");
    Console.WriteLine("3 - Atualizar Álbum");
    Console.WriteLine("4 - Remover Álbum");
    Console.WriteLine("5 - Cadastrar Música");
    Console.WriteLine("6 - Listar Músicas");
    Console.WriteLine("7 - Listar Músicas de um Álbum (INNER JOIN)");
    Console.WriteLine("0 - Sair");
    Console.Write("> ");

    var opt = Console.ReadLine();

    if (opt == "0") break;

    switch (opt)
    {
        case "1": await CreateAlbumAsync(); break;
        case "2": await ListAlbumsAsync(); break;
        case "3": await UpdateAlbumAsync(); break;
        case "4": await DeleteAlbumAsync(); break;
        case "5": await CreateMusicAsync(); break;
        case "6": await ListMusicasAsync(); break;
        case "7": await ListMusicasByAlbumAsync(); break;
        default: Console.WriteLine("Opção inválida."); break;
    }
}

await app.StopAsync();
await webTask;

//
// ===== FUNÇÕES DO CONSOLE =====
//

async Task CreateAlbumAsync()
{
    Console.Write("Nome do álbum: ");
    var nome = (Console.ReadLine() ?? "").Trim();

    Console.Write("Ano: ");
    if (!int.TryParse(Console.ReadLine(), out var ano))
    {
        Console.WriteLine("Ano inválido.");
        return;
    }

    if (string.IsNullOrWhiteSpace(nome))
    {
        Console.WriteLine("Nome é obrigatório.");
        return;
    }

    using var db = new AppDbContext();
    var album = new Album { Nome = nome, Ano = ano };
    db.Albuns.Add(album);
    await db.SaveChangesAsync();

    Console.WriteLine($"Álbum cadastrado! Id: {album.Id}");
}

async Task ListAlbumsAsync()
{
    using var db = new AppDbContext();
    var albuns = await db.Albuns.OrderBy(a => a.Id).ToListAsync();

    if (!albuns.Any())
    {
        Console.WriteLine("Nenhum álbum cadastrado.");
        return;
    }

    Console.WriteLine("\nÁlbuns cadastrados:");
    foreach (var a in albuns)
        Console.WriteLine($"{a.Id} - {a.Nome} ({a.Ano})");
}

async Task UpdateAlbumAsync()
{
    Console.Write("Informe o Id do álbum: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var album = await db.Albuns.FindAsync(id);

    if (album is null)
    {
        Console.WriteLine("Álbum não encontrado.");
        return;
    }

    Console.WriteLine($"Atualizando Álbum {album.Id}: {album.Nome} ({album.Ano})");

    Console.Write("Novo nome (vazio mantém): ");
    var novoNome = Console.ReadLine()?.Trim();
    if (!string.IsNullOrWhiteSpace(novoNome)) album.Nome = novoNome;

    Console.Write("Novo ano (vazio mantém): ");
    var anoStr = Console.ReadLine()?.Trim();
    if (!string.IsNullOrWhiteSpace(anoStr) && int.TryParse(anoStr, out var novoAno))
        album.Ano = novoAno;

    await db.SaveChangesAsync();
    Console.WriteLine("Álbum atualizado!");
}

async Task DeleteAlbumAsync()
{
    Console.Write("Id do álbum: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var album = await db.Albuns.FindAsync(id);

    if (album is null)
    {
        Console.WriteLine("Álbum não encontrado.");
        return;
    }

    db.Albuns.Remove(album);
    await db.SaveChangesAsync();

    Console.WriteLine("Álbum removido.");
}

async Task CreateMusicAsync()
{
    Console.Write("Título: ");
    var titulo = (Console.ReadLine() ?? "").Trim();

    Console.Write("Artista: ");
    var artista = (Console.ReadLine() ?? "").Trim();

    Console.Write("Gênero: ");
    var genero = (Console.ReadLine() ?? "").Trim();

    Console.Write("Id do álbum: ");
    if (!int.TryParse(Console.ReadLine(), out var albumId))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var album = await db.Albuns.FindAsync(albumId);

    if (album is null)
    {
        Console.WriteLine("Álbum não encontrado.");
        return;
    }

    var musica = new Musica
    {
        Titulo = titulo,
        Artista = artista,
        Genero = genero,
        AlbumId = albumId
    };

    db.Musicas.Add(musica);
    await db.SaveChangesAsync();

    Console.WriteLine($"Música cadastrada! Id: {musica.Id}");
}

async Task ListMusicasAsync()
{
    using var db = new AppDbContext();

    var musicas = await db.Musicas
        .Include(m => m.Album)
        .OrderBy(m => m.Id)
        .ToListAsync();

    if (!musicas.Any())
    {
        Console.WriteLine("Nenhuma música cadastrada.");
        return;
    }

    Console.WriteLine("\nMúsicas cadastradas:");
    foreach (var m in musicas)
        Console.WriteLine($"{m.Id} - {m.Titulo} ({m.Artista}) | Álbum: {m.Album?.Nome}");
}

async Task ListMusicasByAlbumAsync()
{
    Console.Write("Id do álbum: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var album = await db.Albuns.FindAsync(id);

    if (album is null)
    {
        Console.WriteLine("Álbum não encontrado.");
        return;
    }

    var musicas = await db.Musicas
        .Where(m => m.AlbumId == id)
        .OrderBy(m => m.Titulo)
        .ToListAsync();

    Console.WriteLine($"\nÁlbum: {album.Nome} ({album.Ano})");

    if (!musicas.Any())
    {
        Console.WriteLine("  (Sem músicas)");
        return;
    }

    foreach (var m in musicas)
        Console.WriteLine($"  - {m.Titulo} ({m.Artista})");
}

