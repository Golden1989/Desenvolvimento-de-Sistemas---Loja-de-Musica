using System.Windows;
using MusicaDB.Gui.Services;
using MusicaDB.Gui.Models;

namespace MusicaDB.Gui;

public partial class MainWindow : Window
{
    private readonly ApiClient _api = new ApiClient();
    private List<MusicaDTO> _listaMusicas = new();

    public MainWindow()
    {
        InitializeComponent();
        CarregarMusicas();
    }

    private async void CarregarMusicas()
    {
        _listaMusicas = await _api.GetMusicasAsync();
        MusicaGrid.ItemsSource = _listaMusicas;
    }

    private void Buscar_Click(object sender, RoutedEventArgs e)
    {
        string termo = SearchBox.Text.ToLower();

        var filtradas = _listaMusicas
            .Where(m => m.Titulo.ToLower().Contains(termo))
            .ToList();

        MusicaGrid.ItemsSource = filtradas;
    }

    private void Adicionar_Click(object sender, RoutedEventArgs e)
    {
        var janela = new Views.EditarMusicaWindow();
        if (janela.ShowDialog() == true)
            CarregarMusicas();
    }

    private void Editar_Click(object sender, RoutedEventArgs e)
    {
        if (MusicaGrid.SelectedItem is not MusicaDTO musica)
        {
            MessageBox.Show("Selecione uma música.");
            return;
        }

        var janela = new Views.EditarMusicaWindow(musica);
        if (janela.ShowDialog() == true)
            CarregarMusicas();
    }

    private async void Excluir_Click(object sender, RoutedEventArgs e)
    {
        if (MusicaGrid.SelectedItem is not MusicaDTO musica)
        {
            MessageBox.Show("Selecione uma música.");
            return;
        }

        if (MessageBox.Show("Tem certeza?", "Confirmação", 
            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        {
            await _api.DeleteMusicaAsync(musica.Id);
            CarregarMusicas();
        }
    }
}
