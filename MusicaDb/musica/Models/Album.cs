using System.ComponentModel.DataAnnotations;

namespace musica.Models;

public class Album
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public int Ano { get; set; }

    // 🔗 Relação: 1 álbum tem muitas músicas
    public List<Musica> Musicas { get; set; } = new();
}

