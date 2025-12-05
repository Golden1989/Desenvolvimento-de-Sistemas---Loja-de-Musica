using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using musica.Data;
using musica.Models;
using musica.DTOs;

namespace musica.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AlbumController : ControllerBase
{
    private readonly AppDbContext _db;

    public AlbumController(AppDbContext db)
    {
        _db = db;
    }

    // GET /api/v1/album
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetAll()
    {
        var albuns = await _db.Albuns
            .Select(a => new AlbumDTO(
                a.Id,
                a.Nome,
                a.Ano
            ))
            .ToListAsync();

        return Ok(albuns);
    }

    // GET /api/v1/album/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AlbumDTO>> GetById(int id)
    {
        var album = await _db.Albuns
            .Include(a => a.Musicas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album is null)
            return NotFound(new { error = "Álbum não encontrado." });

        var dto = new AlbumDTO(
            album.Id,
            album.Nome,
            album.Ano
        );

        return Ok(dto);
    }

    // POST /api/v1/album
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AlbumCreateDTO a)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newAlbum = new Album
        {
            Nome = a.Nome,
            Ano = a.Ano
        };

        _db.Albuns.Add(newAlbum);
        await _db.SaveChangesAsync();

        var dto = new AlbumDTO(
            newAlbum.Id,
            newAlbum.Nome,
            newAlbum.Ano
        );

        return CreatedAtAction(nameof(GetById), new { id = newAlbum.Id }, dto);
    }

    // PUT /api/v1/album/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AlbumCreateDTO a)
    {
        var album = await _db.Albuns.FindAsync(id);

        if (album is null)
            return NotFound(new { error = "Álbum não encontrado." });

        album.Nome = a.Nome;
        album.Ano = a.Ano;

        _db.Albuns.Update(album);
        await _db.SaveChangesAsync();

        var dto = new AlbumDTO(
            album.Id,
            album.Nome,
            album.Ano
        );

        return Ok(dto);
    }

    // DELETE /api/v1/album/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var album = await _db.Albuns.FindAsync(id);

        if (album is null)
            return NotFound(new { error = "Álbum não encontrado." });

        _db.Albuns.Remove(album);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // GET /api/v1/album/1/musicas
    [HttpGet("{id:int}/musicas")]
    public async Task<IActionResult> GetMusicasByAlbum(int id)
    {
        var album = await _db.Albuns
            .Include(a => a.Musicas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (album is null)
            return NotFound(new { error = "Álbum não encontrado." });

        var musicas = album.Musicas
            .Select(m => new MusicaDTO(
                m.Id,
                m.Titulo,
                m.Artista,
                m.Genero,
                m.AlbumId,
                album.Nome
            ));

        return Ok(musicas);
    }
}
