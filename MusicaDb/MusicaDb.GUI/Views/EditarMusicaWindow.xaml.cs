using MusicaDB.Gui.Models;
using MusicaDB.Gui.Services;
using System.Windows;

namespace MusicaDB.Gui.Views;

public partial class EditarMusicaWindow : Window
{
    private readonly ApiClient _api = new();
    private readonly MusicaDTO? _musica;

    public EditarMusicaWindow(MusicaDTO? musica = null)
    {
        InitializeComponent();
        _musica = musica;
        CarregarAlbuns();

        if (_musica != null)
        {
            TxtTitulo.Text = _musica.Titulo;
            TxtArtista.Text = _musica.Artista;
            TxtGenero.Text = _musica.Genero;
            CmbAlbum.SelectedValue = _musica.AlbumId;
        }
    }

    private async void CarregarAlbuns()
    {
        CmbAlbum.ItemsSource = await _api.GetAlbunsAsync();
    }

    private async void Salvar_Click(object sender, RoutedEventArgs e)
    {
        var dto = new MusicaCreateDTO
        {
            Titulo = TxtTitulo.Text,
            Artista = TxtArtista.Text,
            Genero = TxtGenero.Text,
            AlbumId = (int)(CmbAlbum.SelectedValue ?? 0)
        };

        bool ok;

        if (_musica == null)
            ok = await _api.CreateMusicaAsync(dto);
        else
            ok = await _api.UpdateMusicaAsync(_musica.Id, dto);

        if (ok)
        {
            DialogResult = true;
            Close();
        }
        else
        {
            MessageBox.Show("Erro ao salvar.");
        }
    }
}
