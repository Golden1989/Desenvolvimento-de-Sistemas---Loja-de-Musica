using System.Net.Http;
using System.Net.Http.Json;
using MusicaDB.Gui.Models;

namespace MusicaDB.Gui.Services;

public class ApiClient
{
    private readonly HttpClient _http = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5099/api/v1/")
    };

    // LISTA TODAS AS MÚSICAS
    public async Task<List<MusicaDTO>> GetMusicasAsync()
    {
        return await _http.GetFromJsonAsync<List<MusicaDTO>>("musica") 
               ?? new List<MusicaDTO>();
    }

    // LISTA TODOS OS ÁLBUNS
    public async Task<List<AlbumDTO>> GetAlbunsAsync()
    {
        return await _http.GetFromJsonAsync<List<AlbumDTO>>("album")
               ?? new List<AlbumDTO>();
    }

    // CRIAR
    public async Task<bool> CreateMusicaAsync(MusicaCreateDTO dto)
    {
        var response = await _http.PostAsJsonAsync("musica", dto);
        return response.IsSuccessStatusCode;
    }

    // ATUALIZAR
    public async Task<bool> UpdateMusicaAsync(int id, MusicaCreateDTO dto)
    {
        var response = await _http.PutAsJsonAsync($"musica/{id}", dto);
        return response.IsSuccessStatusCode;
    }

    // REMOVER
    public async Task<bool> DeleteMusicaAsync(int id)
    {
        var response = await _http.DeleteAsync($"musica/{id}");
        return response.IsSuccessStatusCode;
    }
}
