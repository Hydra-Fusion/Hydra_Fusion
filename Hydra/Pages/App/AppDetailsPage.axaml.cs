using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Hydra.Converters;
using TheArtOfDev.HtmlRenderer.Avalonia;

namespace Hydra.Pages.App;

public partial class AppDetailsPage : UserControl
{
    public AppDetailsPage()
    {
        InitializeComponent();
    }
    
    public void Next(object source, RoutedEventArgs args)
    {
        slides.Next();
    }
    
    public void Previous(object source, RoutedEventArgs args) 
    {
        slides.Previous();
    }
    
    private void Thumbnail_Click(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is object clickedItem)
        {
            // Encontrar o índice da miniatura clicada
            var items = slides.ItemsSource?.Cast<object>().ToList();
            if (items is null)
                return;

            int index = items.IndexOf(clickedItem);
            if (index >= 0)
                slides.SelectedIndex = index;
        }
    }
}