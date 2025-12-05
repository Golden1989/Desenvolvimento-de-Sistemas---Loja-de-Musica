namespace musica.DTOs;

public record MusicaDTO(
    int Id,
    string Titulo,
    string Artista,
    string? Genero,
    int AlbumId,
    string NomeDoAlbum
);

