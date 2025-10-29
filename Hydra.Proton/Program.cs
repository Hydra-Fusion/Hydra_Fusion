using Hydra.Proton.Models;
using Hydra.Proton.Services;

var props = new UmuProps
{
    GamePath = "/run/media/system/SSD_Games/Hydra/SpongeBob-SquarePants-TCS-SteamRIP.com/SpongeBob SquarePants The Cosmic Shake/CosmicShake.exe",
    ProtonVersion = "GE-Proton10-20",
    SteamInput = true,
    ProtonEnableHidraw = true,
    ProtonLog = true,
};

var sandbox = new SandBoxProps
{
    Networking = false
};

var umu = new UmuServices();

umu.OnLog += (data, log) => Console.WriteLine($"[{data}] - {log}");
 
//await umu.RunSandboxProtonAsync(sandbox, props);

await umu.RunProtonAsync(props);

umu.WaitForClose();
