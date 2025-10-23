using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using musica.Data;
using musica.Models;

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
    public async Task<ActionResult<Musica>> GetById(int id)
    {
        var musica = await _db.Musicas.FindAsync(id);
        if (musica is null)
            return NotFound();

        return Ok(musica);
    }

    // POST /api/v1/musica
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Musica m)
    {
        if (!string.IsNullOrWhiteSpace(m.Titulo) &&
            await _db.Musicas.AnyAsync(x => x.Titulo == m.Titulo))
        {
            return Conflict(new { error = "Já existe uma música com esse título cadastrado." });
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

        if (!string.IsNullOrWhiteSpace(m.Titulo) &&
            await _db.Musicas.AnyAsync(x => x.Titulo == m.Titulo && x.Id != id))
        {
            return Conflict(new { error = "Já existe uma música com esse título cadastrado." });
        }

        if (!await _db.Musicas.AnyAsync(x => x.Id == id))
        {
            return NotFound();
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
}


