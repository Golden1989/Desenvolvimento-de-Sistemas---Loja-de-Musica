namespace MusicaDB.Gui.Models;

public class MusicaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Artista { get; set; } = string.Empty;
    public string? Genero { get; set; }
    public int AlbumId { get; set; }
    public string NomeDoAlbum { get; set; } = string.Empty;
}
