
using SteamStorefrontAPI.Classes;

namespace Hydra.Domain.Models.Internal.Details;

#nullable disable
public class Details
{
    public string Name { get; set; }
        
    public int SteamId { get; set; }
    
    public int RequiredAge { get; set; }
        
    public string ControllerSupport { get; set; }

    public string Description { get; set; }
        
    public string Languages { get; set; }
        
    public string Header { get; set; }

    public Requirements PcRequirements { get; set; }
        
    public Requirements MacRequirements { get; set; }
        
    public Requirements LinuxRequirements { get; set; }
        
    public List<string> Developers { get; set; }
        
    public List<string> Publishers { get; set; }
        
    Platform Platforms { get; set; }
    
    public List<Screenshot> Screenshots { get; set; }

    public Proton Proton { get; set; }
    
    
    
    public string GamaLocation { get; set; }

    public Details(SteamApp app, Proton proton)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        Proton = proton;
        SteamId = app.SteamAppId;
        Name = app.Name ?? string.Empty;
        RequiredAge = app.RequiredAge;
        ControllerSupport = app.ControllerSupport?.ToString() ?? "None";
        Description = app.DetailedDescription ?? string.Empty;
        Languages = app.SupportedLanguages ?? string.Empty;
        Header = app.HeaderImage ?? string.Empty;

        PcRequirements = app.PcRequirements != null
            ? new Requirements
            {
                Minimum = app.PcRequirements.Minimum ?? string.Empty,
                Recommended = app.PcRequirements.Recommended ?? string.Empty
            }
            : new Requirements();

        MacRequirements = app.MacRequirements != null
            ? new Requirements
            {
                Minimum = app.MacRequirements.Minimum ?? string.Empty,
                Recommended = app.MacRequirements.Recommended ?? string.Empty
            }
            : new Requirements();

        LinuxRequirements = app.LinuxRequirements != null
            ? new Requirements
            {
                Minimum = app.LinuxRequirements.Minimum ?? string.Empty,
                Recommended = app.LinuxRequirements.Recommended ?? string.Empty
            }
            : new Requirements();

        Developers = app.Developers ?? new List<string>();
        Publishers = app.Publishers ?? new List<string>();

        Platforms = app.Platforms != null
            ? new Platform
            {
                Linux = app.Platforms.Linux,
                Mac = app.Platforms.Mac,
                Windows = app.Platforms.Windows
            }
            : new Platform();

        Screenshots = app.Screenshots?
            .Where(s => s != null)
            .Select(s => new Screenshot(s.Id, s.PathThumbnail ?? string.Empty, s.PathFull ?? string.Empty))
            .ToList() ?? new List<Screenshot>();
    }
}