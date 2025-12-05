namespace MusicaDB.Gui.Models;

public class MusicaCreateDTO
{
    public string Titulo { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public string? Genero { get; set; }
    public int AlbumId { get; set; }
}
