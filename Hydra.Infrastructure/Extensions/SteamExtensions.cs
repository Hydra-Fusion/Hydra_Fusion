using Hydra.Domain.Models.Steam;

namespace Hydra.Infrastructure.Extensions;

public static class SteamExtensions
{
    public static string ToQuery(this SteamLanguages language)
    {
        return language switch
        {
            SteamLanguages.Japanese => "japanese",
            SteamLanguages.Brazilian => "brazilian",
            SteamLanguages.SimplifiedChinese => "schinese",
            SteamLanguages.TraditionalChinese => "tchinese",
            SteamLanguages.Korean => "koreana",
            SteamLanguages.Thai => "thai",
            SteamLanguages.Arabic => "arabic",
            SteamLanguages.Bulgarian => "bulgarian",
            SteamLanguages.Czech => "czech",
            SteamLanguages.Danish => "danish",
            SteamLanguages.German => "german",
            SteamLanguages.English => "english",
            SteamLanguages.Spanish => "spanish",
            SteamLanguages.SpanishLatinAmerica => "latam",
            SteamLanguages.Greek => "greek",
            SteamLanguages.French => "french",
            _ => ""
        };
    }
}