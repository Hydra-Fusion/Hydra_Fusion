using Avalonia.Controls;
using CommunityToolkit.Mvvm.Messaging;
using Hydra.Extensions.WeakReferenceMessage;
using Hydra.ViewModels;

namespace Hydra;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        
        this.Closing += OnClosing;
        
        WeakReferenceMessenger.Default.AddMessages(this);
    }

    private void OnClosing(object? sender, WindowClosingEventArgs e)
    {
        e.Cancel = true;
        
        this.Hide();
    }
}