
using Hydra.Domain.Converter;
using SteamStorefrontAPI.Classes;

namespace Hydra.Domain.Models.Internal.Details;

#nullable disable
public class Details
{
    public string Name { get; set; }
    
    public string LibraryHero { get; set; }
        
    public string Id { get; set; }
    
    public string Store { get; set; }
    
    public int RequiredAge { get; set; }
        
    public ControllerSupport ControllerSupport { get; set; }

    public string Description { get; set; }
        
    public string Languages { get; set; }
        
    public string Header { get; set; }

    public Requirements PcRequirements { get; set; }
        
    public Requirements MacRequirements { get; set; }
        
    public Requirements LinuxRequirements { get; set; }
        
    public List<string> Developers { get; set; }
        
    public List<string> Publishers { get; set; }
        
    public Platform Platforms { get; set; }
    
    public List<Screenshot> Screenshots { get; set; }

    public Proton Proton { get; set; }
    
    
    
    public string GamaLocation { get; set; }

    public Details(SteamApp app, Proton proton)
    {
        if (app == null)
            throw new ArgumentNullException(nameof(app));

        LibraryHero = $"https://shared.steamstatic.com/store_item_assets/steam/apps/{app.SteamAppId}/library_hero.jpg";
        Proton = proton;
        Proton.Tier = Proton.Tier.ToUpper();
        
        Id = app.SteamAppId.ToString();
        Name = app.Name ?? string.Empty;
        RequiredAge = app.RequiredAge;
        ControllerSupport = (ControllerSupport)
            (app.ControllerSupport ?? SteamStorefrontAPI.Classes.ControllerSupport.Partial);
        Description = HtmlToMarkdown.ConvertSteamDescription(app.DetailedDescription) ?? string.Empty;
        Languages = app.SupportedLanguages ?? string.Empty;
        Header = app.HeaderImage ?? string.Empty;

        PcRequirements = app.PcRequirements != null
            ? new Requirements
            {
                Minimum = HtmlToMarkdown.ConvertSteamDescription(app.PcRequirements.Minimum) ?? string.Empty,
                Recommended = HtmlToMarkdown.ConvertSteamDescription(app.PcRequirements.Recommended) ?? string.Empty
            }
            : new Requirements();

        MacRequirements = app.MacRequirements != null
            ? new Requirements
            {
                Minimum = HtmlToMarkdown.ConvertSteamDescription(app.MacRequirements.Minimum) ?? string.Empty,
                Recommended = HtmlToMarkdown.ConvertSteamDescription(app.MacRequirements.Recommended) ?? string.Empty
            }
            : new Requirements();

        LinuxRequirements = app.LinuxRequirements != null
            ? new Requirements
            {
                Minimum = HtmlToMarkdown.ConvertSteamDescription(app.LinuxRequirements.Minimum) ?? string.Empty,
                Recommended = HtmlToMarkdown.ConvertSteamDescription(app.LinuxRequirements.Recommended) ?? string.Empty
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