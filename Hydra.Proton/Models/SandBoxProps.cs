namespace Hydra.Proton.Models;

public class SandBoxProps
{
    public bool Networking { get; set; } = true;
    public bool Sound { get; set; } = true;

    public string Root => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Prefix/Hydra/drive_c");

    public Dictionary<string, string> IncludePaths { get; private set; }
        = new Dictionary<string, string>()
        {
            { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Prefix/Hydra/drive_c"), "/C \"" }
        };

    public Dictionary<string, string> Envs { get; set; } = new Dictionary<string, string>()
    {
        
    };

    public List<string> PathReadOnly { get; set; } = new()
    {
        "/usr",
        "/lib",
        "/lib64",
        "/etc",
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Libs/Umu"),
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Proton")
    };
    
    public List<(string Flag, string Path1, string Path2)> SystemMounts { get; private set; } = new()
    {
        ("--proc", "/proc", null),
        ("--dev", "/dev", null),
        ("--tmpfs", "/tmp", null)
    };

    public List<string> BuildProps()
    {
        var props = new List<string>();

        // Pastas somente leitura
        foreach (var path in PathReadOnly)
            props.AddRange(new[] { "--ro-bind", path, path });

        // Pastas bind normais
        foreach (var kv in IncludePaths)
            props.AddRange(new[] { "--bind", kv.Key, kv.Value });

        // Montagens do sistema
        foreach (var mount in SystemMounts)
        {
            if (mount.Path2 == null)
                props.AddRange(new[] { mount.Flag, mount.Path1 });
            else
                props.AddRange(new[] { mount.Flag, mount.Path1, mount.Path2 });
        }

        // Rede e som
        if (Networking)
            props.Add("--share-net");

        if (Sound)
        {
            props.AddRange(new[] { "--bind", "/run/user/1000/pulse", "/run/user/1000/pulse \"" });
            props.AddRange(new[] { "--bind", "/run/user/1000/pipewire-0", "/run/user/1000/pipewire-0 \"" });
        }

        // Variáveis de ambiente
        foreach (var env in Envs)
            props.AddRange(new[] { "--setenv", env.Key, env.Value });

        // Fim dos argumentos do bwrap
        props.Add("--");

        return props;
    }

}