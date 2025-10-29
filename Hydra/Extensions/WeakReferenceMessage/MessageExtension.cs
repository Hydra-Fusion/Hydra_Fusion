using CommunityToolkit.Mvvm.Messaging;
using Hydra.Message;
using Hydra.ViewModels.App;
using Hydra.Windows.AppDetails.SetupOptions;

namespace Hydra.Extensions.WeakReferenceMessage;

public static class MessageExtension
{
    public static void AddMessages(this WeakReferenceMessenger maneger, MainWindow window)
    {
        maneger.Register<MainWindow, AppDetailsConfigMessage>(window, static (w, m) =>
        {
            var dialog = new SetupOptionsWindow();
            
            m.Reply( dialog.ShowDialog<AppDetailsViewModel?>(w));
        });
    }
}