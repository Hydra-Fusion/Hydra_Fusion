using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace Hydra.Infrastructure.Localizer;

public class Localizer : INotifyPropertyChanged
{
    private const string IndexerName = "Item";
    private const string IndexerArrayName = "Item[]";
    private Dictionary<string, string> m_Strings = null;

    public bool LoadLanguage(string language)
    {
        Language = language;

        var path = Path.Combine(AppContext.BaseDirectory, "Assets", "i18n", $"{language}.json");
        
        var json = File.ReadAllText(path);
        
        if (File.Exists(path)) {
            m_Strings = FlattenJson(json);
            
            Invalidate();

            return true;
        }
        return false;
    }

    public string Language { get; private set; }

    public string this[string key]
    {
        get
        {
            if (m_Strings != null && m_Strings.TryGetValue(key, out string res))
                return res.Replace("\\n", "\n");

            return $"{Language}:{key}";
        }
    }

    public static Localizer Instance { get; set; } = new Localizer();
    public event PropertyChangedEventHandler PropertyChanged;

    public void Invalidate()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
    }
    
    public static Dictionary<string, string> FlattenJson(string json)
    {
        var jObj = JObject.Parse(json);
        var result = new Dictionary<string, string>();
        FlattenToken(jObj, "", result);
        return result;
    }

    private static void FlattenToken(JToken token, string prefix, Dictionary<string, string> result)
    {
        if (token.Type == JTokenType.Object)
        {
            foreach (var prop in token.Children<JProperty>())
            {
                var newPrefix = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}_{prop.Name}";
                FlattenToken(prop.Value, newPrefix, result);
            }
        }
        else if (token.Type == JTokenType.Array)
        {
            int i = 0;
            foreach (var item in token.Children())
            {
                var newPrefix = $"{prefix}_{i}";
                FlattenToken(item, newPrefix, result);
                i++;
            }
        }
        else
        {
            result[prefix] = token.ToString().Replace("\\n", "\n");
        }
    }
}