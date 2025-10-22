using System.Diagnostics;
using System.Text.Json;
using GameFinder.StoreHandlers.Steam;
using Hydra.Proton.Models;
using NexusMods.Paths;
using SharpCompress.Common;
using SharpCompress.Readers;

namespace Hydra.Proton.Services;

public class UmuServices
{
    public event Action<DateTime, string>? OnLog;
    private SteamHandler? _steamHandler = new SteamHandler(FileSystem.Shared, null, null);
    private readonly string _steamPath;

    private Process? Process { get; set; } = null;

    public UmuServices()
    {
        var game = _steamHandler.FindAllGames().FirstOrDefault();

        _steamPath = game.AsT0.SteamPath.Directory ?? "";
    }
    
    public async Task RunProtonAsync(UmuProps props)
    {
        if (props == null) throw new ArgumentNullException(nameof(props));

        static string Quote(string s) => $"\"{s}\"";

        await EnsureExecutableAsync(props.RunPath);
        await EnsureExecutableAsync(Path.Combine(props.ProtonPath, "proton"));

        var envs = await ConfigureProps(props);

        var startInfo = new ProcessStartInfo
        {
            FileName = "python3",
            UseShellExecute = false,
            CreateNoWindow = props.HiddenConsole,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = Path.GetDirectoryName(props.RunPath) ?? Environment.CurrentDirectory
        };

        try
        {
            startInfo.ArgumentList.Clear();
            startInfo.ArgumentList.Add(props.RunPath);
            startInfo.ArgumentList.Add(props.GamePath);
            if (!string.IsNullOrWhiteSpace(props.Args))
                startInfo.ArgumentList.Add(props.Args);
        }
        catch
        {
            startInfo.Arguments = Quote(props.RunPath) + " " + Quote(props.GamePath) + (string.IsNullOrWhiteSpace(props.Args) ? "" : " " + props.Args);
        }

        foreach (var env in envs)
            if (env.Value is not null)
                startInfo.Environment[env.Key] = env.Value;

        Process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        Process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLog?.Invoke(DateTime.Now, e.Data);
        };
        Process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLog?.Invoke(DateTime.Now, $"[ERR] {e.Data}");
        };
        Process.Exited += (_, _) =>
        {
            OnLog?.Invoke(DateTime.Now, "Processo Proton finalizado.");
        };

        if (!Process.Start())
            throw new Exception("Falha ao iniciar o Proton.");

        Process.BeginOutputReadLine();
        Process.BeginErrorReadLine();
    }

    public async Task RunSandboxProtonAsync(SandBoxProps sandbox, UmuProps props)
    {
        if(props == null) throw new ArgumentNullException(nameof(props));
        if(sandbox == null) throw new ArgumentNullException(nameof(sandbox));

        props.IsSandBox = true;
        
        await EnsureExecutableAsync(props.RunPath);
        
        var GamePath = Path.Combine(
            "/c/users/Public/Games", 
            Path.GetFileName(Path.GetDirectoryName(props.GamePath) ?? "Game"));
        
        sandbox.IncludePaths.Add(Path.GetDirectoryName(props.GamePath), GamePath);
        
        foreach (var env in await ConfigureProps(props))
            sandbox.Envs.Add(env.Key, env.Value);
        
        var sandboxProps = sandbox.BuildProps();
        
        sandboxProps.AddRange(new List<string>()
        {
            "python3",
            props.RunPath,
            GamePath,
            props.Args
        });

        var startInfo = new ProcessStartInfo
        {
            FileName = "bwrap",
            CreateNoWindow = props.HiddenConsole,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = Path.GetDirectoryName(props.RunPath) ?? Environment.CurrentDirectory
        };

        foreach (var prop in sandboxProps)
            startInfo.ArgumentList.Add(prop);
        
        Process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        Process.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLog?.Invoke(DateTime.Now, e.Data);
        };
        Process.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                OnLog?.Invoke(DateTime.Now, $"[ERR] {e.Data}");
        };
        Process.Exited += (_, _) =>
        {
            OnLog?.Invoke(DateTime.Now, "Processo Proton finalizado.");
        };

        if (!Process.Start())
            throw new Exception("Falha ao iniciar o Proton.");

        Process.BeginOutputReadLine();
        Process.BeginErrorReadLine();
    }
    
    public void WaitForClose()
    {
        if (Process != null)
            Process.WaitForExit();
    }

    public async Task WaitForCloseAsync()
    {
        if (Process != null)
            await Process.WaitForExitAsync();
    }
    
    public void Close()
    {
        if (Process != null)
        {
            Process.Kill();
            Process.Dispose();
            Process = null;
        }
    }
    
    private async Task<IDictionary<string, string?>> ConfigureProps(UmuProps props)
    {
        if(!File.Exists(props.RunPath))
            throw new FileNotFoundException($"umu-run não encontrado em: {props.RunPath}");
        
        if(!File.Exists(props.GamePath))
            throw new FileNotFoundException($"Executável não encontrado: {props.GamePath}");

        if (!Directory.Exists(props.ProtonPath))
            await DownloadAndExtractProtonAsync(props.ProtonVersion);

        Directory.CreateDirectory(props.WinePrefix);

        var envs = props.ToDictionary();

        if (props.UseSteamCompatClient && !string.IsNullOrWhiteSpace(_steamPath))
            envs["STEAM_COMPAT_CLIENT_INSTALL_PATH"] = _steamPath;

        return envs;
    }

    public async Task<string> DownloadAndExtractProtonAsync(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            throw new ArgumentException("A versão do Proton-GE deve ser especificada (ex: GE-Proton10-21).", nameof(version));

        const string apiUrl = "https://api.github.com/repos/GloriousEggroll/proton-ge-custom/releases";

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Hydra-Proton");

        OnLog?.Invoke(DateTime.Now, $"Buscando release {version} do Proton-GE...");
        var json = await client.GetStringAsync(apiUrl);
        using var doc = JsonDocument.Parse(json);

        JsonElement? targetRelease = null;
        foreach (var release in doc.RootElement.EnumerateArray())
        {
            if (release.GetProperty("tag_name").GetString() == version)
            {
                targetRelease = release;
                break;
            }
        }

        if (targetRelease == null)
            throw new Exception($"Nenhuma release encontrada para a versão {version}.");

        var asset = targetRelease.Value
            .GetProperty("assets")
            .EnumerateArray()
            .FirstOrDefault(a => a.GetProperty("name").GetString()?.EndsWith(".tar.gz") == true);

        if (asset.ValueKind == JsonValueKind.Undefined)
            throw new Exception($"Nenhum arquivo .tar.gz encontrado na release {version}.");

        var url = asset.GetProperty("browser_download_url").GetString();
        if (string.IsNullOrEmpty(url))
            throw new Exception("URL de download do Proton não encontrada.");

        var protonBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Proton");
        Directory.CreateDirectory(protonBasePath);

        var fileName = Path.Combine(protonBasePath, $"{version}.tar.gz");
        var extractDir = Path.Combine(protonBasePath, version);

        OnLog?.Invoke(DateTime.Now, $"Baixando Proton-GE {version} de {url}...");
        var data = await client.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(fileName, data);

        if (Directory.Exists(extractDir))
        {
            OnLog?.Invoke(DateTime.Now, $"Removendo diretório de extração anterior: {extractDir}");
            Directory.Delete(extractDir, true);
        }

        Directory.CreateDirectory(extractDir);

        OnLog?.Invoke(DateTime.Now, $"Extraindo Proton-GE {version} para {extractDir}...");
        ExtractTarGz(fileName, extractDir);

        try
        {
            var protonFile = Path.Combine(extractDir, "proton");
            var wineFile = Path.Combine(extractDir, "wine");
            if (File.Exists(protonFile))
            {
                var process = new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{protonFile}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };
                using var proc = Process.Start(process);
                var error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                    OnLog?.Invoke(DateTime.Now, $"Aviso: Falha ao definir permissões para {protonFile}: {error}");
            }
            if (File.Exists(wineFile))
            {
                var process = new ProcessStartInfo
                {
                    FileName = "chmod",
                    Arguments = $"+x \"{wineFile}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true
                };
                using var proc = Process.Start(process);
                var error = proc.StandardError.ReadToEnd();
                proc.WaitForExit();
                if (proc.ExitCode != 0)
                    OnLog?.Invoke(DateTime.Now, $"Aviso: Falha ao definir permissões para {wineFile}: {error}");
            }
            OnLog?.Invoke(DateTime.Now, "Permissões de execução definidas para proton e wine.");
        }
        catch (Exception ex)
        {
            OnLog?.Invoke(DateTime.Now, $"Aviso: Não foi possível definir permissões de execução: {ex.Message}");
        }

        File.Delete(fileName);
        OnLog?.Invoke(DateTime.Now, $"Arquivo {fileName} removido após extração.");

        OnLog?.Invoke(DateTime.Now, $"✅ Proton-GE {version} baixado e extraído com sucesso para {extractDir}.");
        
        return extractDir;
    }

    public static void ExtractTarGz(string sourceFile, string destination)
    {
        if (!File.Exists(sourceFile))
            throw new FileNotFoundException($"Arquivo .tar.gz não encontrado: {sourceFile}");

        using var stream = File.OpenRead(sourceFile);
        using var reader = ReaderFactory.Open(stream);

        Directory.CreateDirectory(destination);

        while (reader.MoveToNextEntry())
        {
            var parts = reader.Entry.Key.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // remove a primeira pasta se for igual ao nome da pasta destino
            if (parts.Length > 0 && parts[0].Equals(Path.GetFileName(destination), StringComparison.OrdinalIgnoreCase))
            {
                parts = parts.Skip(1).ToArray();
            }

            if (parts.Length == 0)
                continue;

            string fullPath = Path.Combine(destination, Path.Combine(parts));

            if (reader.Entry.IsDirectory)
            {
                Directory.CreateDirectory(fullPath);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
                reader.WriteEntryToFile(fullPath, new ExtractionOptions
                {
                    ExtractFullPath = true,
                    Overwrite = true
                });
            }
        }
    }

    private async Task EnsureExecutableAsync(string filePath)
    {
        try
        {
            var chmod = new ProcessStartInfo("chmod", $"+x \"{filePath}\"")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var proc = Process.Start(chmod);
            await proc.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            OnLog?.Invoke(DateTime.Now, $"Erro ao aplicar permissões de execução: {ex.Message}");
            throw;
        }
    }
}