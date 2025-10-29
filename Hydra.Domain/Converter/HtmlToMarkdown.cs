using System.Text.RegularExpressions;
using ReverseMarkdown;

namespace Hydra.Domain.Converter;

public static class HtmlToMarkdown
{
    public static string ConvertSteamDescription(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
        {
            System.Diagnostics.Debug.WriteLine("HTML de entrada vazio ou nulo.");
            return string.Empty;
        }

        // 1️⃣ Remove tags desnecessárias, preservando títulos
        html = Regex.Replace(html, @"<(span|div|br|p)[^>]*>", "\n", RegexOptions.IgnoreCase);
        html = Regex.Replace(html, @"</(span|div|p)>", "\n", RegexOptions.IgnoreCase);

        // 2️⃣ Adiciona quebras de linha após tags de título (<h1> a <h6>)
        html = Regex.Replace(html, @"</h[1-6]>", "$0\n\n", RegexOptions.IgnoreCase);

        // 3️⃣ Converte imagens para Markdown, ignorando AVIF e adicionando espaçamento
        html = Regex.Replace(html, @"<img[^>]*src\s*=\s*[""']([^""']+)[""'][^>]*>", match =>
        {
            var src = match.Groups[1].Value.Trim();
            src = src.Replace("&amp;", "&"); // Corrige entidades HTML

            // Ignora imagens AVIF
            if (src.Contains(".avif", StringComparison.OrdinalIgnoreCase))
            {
                System.Diagnostics.Debug.WriteLine($"Ignorando imagem AVIF: {src}");
                return string.Empty;
            }

            // Adiciona espaçamento antes e depois da imagem
            return $"\n\n![image]({src})\n\n";
        }, RegexOptions.IgnoreCase);

        // 4️⃣ Remove atributos desnecessários
        html = Regex.Replace(html, @"\s(class|width|height|style|alt|data-[^=]+)\s*=\s*[""'][^""']*[""']", "", RegexOptions.IgnoreCase);

        // 5️⃣ Converte HTML para Markdown
        var config = new Config
        {
            GithubFlavored = true,
            UnknownTags = Config.UnknownTagsOption.Bypass,
            SmartHrefHandling = true,
            ListBulletChar = '-'
        };

        var converter = new ReverseMarkdown.Converter(config);
        var markdown = converter.Convert(html);

        // 6️⃣ Corrige URLs e formatação Markdown
        markdown = markdown
            .Replace(@"\_", "_")
            .Replace(@"\?", "?")
            .Replace(@"\&", "&")
            .Replace(@"\(", "(")
            .Replace(@"\)", ")")
            .Replace("\n\n\n", "\n\n") // Evita múltiplas quebras de linha
            .Trim();

        // 7️⃣ Adiciona espaçamento final após títulos no Markdown
        markdown = Regex.Replace(markdown, @"^(#+ .+?)$", "$1\n\n", RegexOptions.Multiline);

        return markdown;
    }
}