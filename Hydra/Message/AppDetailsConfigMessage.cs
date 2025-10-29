using CommunityToolkit.Mvvm.Messaging.Messages;
using Hydra.ViewModels.App;

namespace Hydra.Message;

public class AppDetailsConfigMessage : AsyncRequestMessage<AppDetailsViewModel?>
{
    public string AppId { get; set; }
    
    public string AppName { get; set; }

    public AppDetailsConfigMessage(string appId, string appName)
    {
        AppId = appId;
        AppName = appName;
    }
}