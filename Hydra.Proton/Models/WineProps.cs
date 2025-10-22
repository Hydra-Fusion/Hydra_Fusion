namespace Hydra.Proton.Models;

public class WineProps
{
    #region # Variáveis essenciais

    public required string WinePrefix { get; set; }
    public required Arch WineArch { get; set; }
    public required string WineDebug { get; set; }
    public required string LcAll { get; set; } = "C.UTF-8";

    #endregion

    #region # Runtime do Proton / Bottles

    public string? ProtonEacRuntime { get; set; }

    #endregion
}

public enum Arch
{
    Arch32,
    Arch64,
}