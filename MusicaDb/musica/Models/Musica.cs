namespace musica.Models;

using System.ComponentModel.DataAnnotations;

public class Musica
{
    public int Id { get; set; }
    public string Titulo { get; set; } = " ";
    public string Artista { get; set; } = " ";

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;


}

