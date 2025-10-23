using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using musica.Data;
using musica.Models;

var builder = WebApplication.CreateBuilder(args);

// Porta fixa (opcional, facilita testes)
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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

var webTask = app.RunAsync();
Console.WriteLine("API online em http://localhost:5099 (Swagger em /swagger)");

Console.WriteLine("== MusicaDbLab ==");
Console.WriteLine("Console + API executando juntos!");

while (true)
{
    Console.WriteLine();
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1 - Cadastrar música");
    Console.WriteLine("2 - Listar músicas");
    Console.WriteLine("3 - Atualizar música (por Id)");
    Console.WriteLine("4 - Remover música (por Id)");
    Console.WriteLine("0 - Sair");
    Console.Write("> ");

    var opt = Console.ReadLine();

    if (opt == "0") break;

    switch (opt)
    {
        case "1": await CreateMusicaAsync(); break;
        case "2": await ListMusicasAsync(); break;
        case "3": await UpdateMusicaAsync(); break;
        case "4": await DeleteMusicaAsync(); break;
        default: Console.WriteLine("Opção inválida."); break;
    }
}

await app.StopAsync();
await webTask;

// ===================== MÉTODOS AUXILIARES ===================== //

async Task CreateMusicaAsync()
{
    Console.Write("Título: ");
    var titulo = (Console.ReadLine() ?? "").Trim();

    Console.Write("Artista: ");
    var artista = (Console.ReadLine() ?? "").Trim();

    if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(artista))
    {
        Console.WriteLine("Título e Artista são obrigatórios.");
        return;
    }

    using var db = new AppDbContext();
    var exists = await db.Musicas.AnyAsync(m => m.Titulo == titulo);
    if (exists)
    {
        Console.WriteLine("Já existe uma música com esse título cadastrada.");
        return;
    }

    var musica = new Musica { Titulo = titulo, Artista = artista, DataCadastro = DateTime.UtcNow };
    db.Musicas.Add(musica);
    await db.SaveChangesAsync();
    Console.WriteLine($"Cadastrada com sucesso! Id: {musica.Id}");
}

async Task ListMusicasAsync()
{
    using var db = new AppDbContext();
    var musicas = await db.Musicas.OrderBy(m => m.Id).ToListAsync();

    if (musicas.Count == 0)
    {
        Console.WriteLine("Nenhuma música encontrada.");
        return;
    }

    Console.WriteLine("Id | Título               | Artista               | DataCadastro (UTC)");
    foreach (var m in musicas)
        Console.WriteLine($"{m.Id,2} | {m.Titulo,-20} | {m.Artista,-20} | {m.DataCadastro:yyyy-MM-dd HH:mm:ss}");
}

async Task UpdateMusicaAsync()
{
    Console.Write("Informe o Id da música a atualizar: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var musica = await db.Musicas.FirstOrDefaultAsync(m => m.Id == id);
    if (musica is null)
    {
        Console.WriteLine("Música não encontrada.");
        return;
    }

    Console.WriteLine($"Atualizando Id {musica.Id}. Deixe em branco para manter.");
    Console.WriteLine($"Título atual : {musica.Titulo}");
    Console.Write("Novo título  : ");
    var newTitulo = (Console.ReadLine() ?? "").Trim();

    Console.WriteLine($"Artista atual: {musica.Artista}");
    Console.Write("Novo artista : ");
    var newArtista = (Console.ReadLine() ?? "").Trim();

    if (!string.IsNullOrWhiteSpace(newTitulo)) musica.Titulo = newTitulo;
    if (!string.IsNullOrWhiteSpace(newArtista)) musica.Artista = newArtista;

    await db.SaveChangesAsync();
    Console.WriteLine("Música atualizada com sucesso.");
}

async Task DeleteMusicaAsync()
{
    Console.Write("Informe o Id da música a remover: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Id inválido.");
        return;
    }

    using var db = new AppDbContext();
    var musica = await db.Musicas.FirstOrDefaultAsync(m => m.Id == id);
    if (musica is null)
    {
        Console.WriteLine("Música não encontrada.");
        return;
    }

    db.Musicas.Remove(musica);
    await db.SaveChangesAsync();
    Console.WriteLine("Música removida com sucesso.");
}

