using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using musica.Data;
using musica.Models;
using musica.DTOs;



namespace musica.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class MusicaController : ControllerBase
{
    private readonly AppDbContext _db;

    public MusicaController(AppDbContext db)
    {
        _db = db;
    }

   
    // GET /api/v1/musica/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MusicaDTO>> GetById(int id)
    {
        var musica = await _db.Musicas
            .Include(m => m.Album)   // carrega apenas o album
            .FirstOrDefaultAsync(m => m.Id == id);

        if (musica is null)
            return NotFound(new { error = "Música não encontrada." });

        var dto = new MusicaDTO(
            musica.Id,
            musica.Titulo,
            musica.Artista,
            musica.Genero,
            musica.AlbumId,
            musica.Album?.Nome ?? string.Empty
        );

        return Ok(dto);
    }


    // POST /api/v1/musica
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Musica m)
    {
        // Verifica título duplicado
        if (!string.IsNullOrWhiteSpace(m.Titulo) &&
            await _db.Musicas.AnyAsync(x => x.Titulo == m.Titulo))
        {
            return Conflict(new { error = "Já existe uma música com esse título cadastrado." });
        }

        // Verifica se o álbum existe
        if (!await _db.Albuns.AnyAsync(a => a.Id == m.AlbumId))
        {
            return BadRequest(new { error = "O álbum informado não existe." });
        }

        _db.Musicas.Add(m);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = m.Id }, m);
    }

    // PUT /api/v1/musica/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Musica m)
    {
        m.Id = id;

        // Verifica título duplicado
        if (!string.IsNullOrWhiteSpace(m.Titulo) &&
            await _db.Musicas.AnyAsync(x => x.Titulo == m.Titulo && x.Id != id))
        {
            return Conflict(new { error = "Já existe uma música com esse título cadastrado." });
        }

        // Verifica se a música existe
        if (!await _db.Musicas.AnyAsync(x => x.Id == id))
        {
            return NotFound();
        }

        // Verifica se o novo álbum existe
        if (!await _db.Albuns.AnyAsync(a => a.Id == m.AlbumId))
        {
            return BadRequest(new { error = "O álbum informado não existe." });
        }

        _db.Musicas.Update(m);
        await _db.SaveChangesAsync();

        return Ok();
    }

    // DELETE /api/v1/musica/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var musica = await _db.Musicas.FindAsync(id);

        if (musica is null)
            return NotFound();

        _db.Musicas.Remove(musica);
        await _db.SaveChangesAsync();

        return NoContent();
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var musicas = await _db.Musicas
            .Include(m => m.Album)
            .Select(m => new MusicaDTO(
                m.Id,
                m.Titulo,
                m.Artista,
                m.Genero,
                m.AlbumId,
                m.Album!.Nome
            ))
            .ToListAsync();

        return Ok(musicas);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string titulo)
    {
        var list = await _db.Musicas
            .Where(m => m.Titulo.Contains(titulo))
            .ToListAsync();

        return Ok(list);
    }



}

