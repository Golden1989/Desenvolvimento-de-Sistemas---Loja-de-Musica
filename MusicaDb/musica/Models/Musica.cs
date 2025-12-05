using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace musica.Models;

public class Musica
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Artista { get; set; } = string.Empty;

    [MaxLength(60)]
    public string? Genero { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    // 🔗 Chave estrangeira do álbum
    public int AlbumId { get; set; }

    // 🔗 Relação com Album (N músicas -> 1 álbum)
    public Album? Album { get; set; }
}



