using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Hydra.Proton.Models;

/// <summary>
/// Configura as propriedades para execução de jogos ou aplicativos Windows no Linux usando o UMU (Unified Linux Wine Game Launcher)
/// com Proton-GE. Mapeia configurações para variáveis de ambiente do UMU/Proton, permitindo personalizar o ambiente de execução.
/// Requer os arquivos <c>umu-run</c> e <c>umu-run.py</c> em <c>Assets/Libs/Umu/</c> e o Proton-GE em <c>Assets/Proton/</c>.
/// </summary>
public class UmuProps
{
    /// <summary>
    /// Obtém o caminho do script <c>umu-run</c>, o ponto de entrada do UMU portátil.
    /// </summary>
    /// <remarks>
    /// Calculado como <c>Assets/Libs/Umu/umu-run</c> relativo ao diretório do aplicativo.
    /// O <c>umu-run</c> é um script bash que configura o Steam Linux Runtime e chama o <c>umu-run.py</c>.
    /// Marcado com <c>[JsonIgnore]</c> para evitar serialização em JSON.
    /// </remarks>
    [JsonIgnore]
    public string RunPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Libs", "Umu/umu-run");

    /// <summary>
    /// Caminho do executável do jogo ou aplicativo (ex: .exe).
    /// </summary>
    /// <remarks>
    /// Exemplo: <c>/home/Jayme/Downloads/rufus-4.11.exe</c>. Obrigatório para execução.
    /// Use o botão "Browse" na interface para selecionar o arquivo.
    /// </remarks>
    public string GamePath { get; set; } = "";

    /// <summary>
    /// Argumentos adicionais para o executável (ex: -opengl, -windowed).
    /// </summary>
    /// <remarks>
    /// Passados diretamente ao comando <c>umu-run</c> após o <c>GamePath</c>.
    /// Exemplo: <c>-opengl</c> para forçar modo OpenGL em alguns jogos.
    /// </remarks>
    public string Args { get; set; } = "";

    /// <summary>
    /// ID do jogo no umu-database (ex: umu-default, epic_123).
    /// </summary>
    /// <remarks>
    /// Usado pelo UMU para aplicar configurações específicas do jogo.
    /// Padrão: <c>umu-default</c> para aplicativos genéricos (ex: Rufus).
    /// Consulte o umu-database (https://github.com/Open-Wine-Components/umu-database) para IDs de jogos.
    /// Mapeia para a variável de ambiente <c>GAMEID</c>.
    /// </remarks>
    public string GameId { get; set; } = "umu-default";

    /// <summary>
    /// Caminho do prefixo Wine onde configurações e saves são armazenados.
    /// </summary>
    /// <remarks>
    /// Padrão: <c>Assets/Prefix/Hydra</c> relativo ao diretório do aplicativo.
    /// Exemplo alternativo: <c>~/rufus-prefix</c>. O prefixo contém o <c>drive_c</c> e registros do Wine.
    /// Mapeia para a variável de ambiente <c>WINEPREFIX</c>.
    /// </remarks>
    public string WinePrefix { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Prefix", "Hydra");

    /// <summary>
    /// Plataforma ou loja do jogo (ex: none, egs, gog, steam).
    /// </summary>
    /// <remarks>
    /// Usado pelo UMU para aplicar fixes de compatibilidade (ex: DRM para Epic Games com <c>egs</c>).
    /// Padrão: <c>none</c> para aplicativos genéricos como Rufus.
    /// Mapeia para a variável de ambiente <c>STORE</c>.
    /// </remarks>
    public string Store { get; set; } = "none";

    /// <summary>
    /// Obtém o caminho do diretório do Proton-GE (ex: Assets/Proton/GE-Proton10-21).
    /// </summary>
    /// <remarks>
    /// Calculado com base na <c>ProtonVersion</c>. Padrão: <c>GE-Proton10-21</c>.
    /// O diretório deve conter os binários do Proton-GE (ex: <c>proton</c>, <c>wine</c>).
    /// Mapeia para a variável de ambiente <c>PROTONPATH</c>.
    /// </remarks>
    public string ProtonPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets/Proton", ProtonVersion ?? "GE-Proton10-21");
    
    public bool IsSandBox { get; set; } = false;

    /// <summary>
    /// Versão do Proton-GE a ser usada (ex: GE-Proton10-21).
    /// </summary>
    /// <remarks>
    /// Define o subdiretório em <c>Assets/Proton/</c>. Se não especificado, usa <c>GE-Proton10-21</c>.
    /// Exemplo: <c>GE-Proton9-14</c> para outra versão do Proton.
    /// </remarks>
    public string ProtonVersion { get; set; }

    /// <summary>
    /// Ativa logs detalhados do Proton-GE.
    /// </summary>
    /// <remarks>
    /// Quando <c>true</c>, gera logs em <c>~/.local/share/umu/proton.log</c> ou no <c>WINEPREFIX</c>.
    /// Útil para depurar erros de inicialização, GPU ou DLLs.
    /// Mapeia para a variável de ambiente <c>PROTON_LOG=1</c>.
    /// </remarks>
    public bool ProtonLog { get; set; } = false;

    /// <summary>
    /// Modo de renderização para gráficos (Dxvk para Vulkan, Wine3d para OpenGL).
    /// </summary>
    /// <remarks>
    /// <c>Dxvk</c> (padrão) usa Vulkan para melhor desempenho em GPUs modernas.
    /// <c>Wine3d</c> usa OpenGL, útil para GPUs antigas ou jogos com problemas no Vulkan.
    /// Mapeia para a variável de ambiente <c>PROTON_USE_WINED3D=1</c> (quando Wine3d).
    /// </remarks>
    public RenderMode RenderMode { get; set; } = RenderMode.Dxvk;

    /// <summary>
    /// Tipo de sincronização para performance multithreading.
    /// </summary>
    /// <remarks>
    /// <c>Async</c> (padrão): Desativa esync e fsync para máxima compatibilidade.
    /// <c>Esync</c>: Melhora performance em jogos multithreaded, mas pode causar crashes.
    /// <c>Fsync</c>: Alta performance, requer kernel moderno (padrão no Bazzite).
    /// Mapeia para <c>PROTON_NO_ESYNC</c> e <c>PROTON_NO_FSYNC</c>.
    /// </remarks>
    public SyncSelect SyncUse { get; set; } = SyncSelect.Async;

    /// <summary>
    /// Permite que jogos 32-bit usem até 4GB de RAM.
    /// </summary>
    /// <remarks>
    /// Evita crashes por falta de memória em jogos antigos (ex: Skyrim 32-bit).
    /// Mapeia para a variável de ambiente <c>PROTON_FORCE_LARGE_ADDRESS_AWARE=1</c>.
    /// </remarks>
    public bool ForceLargeAddress { get; set; } = false;

    /// <summary>
    /// Gera scripts de debug com comandos do Proton.
    /// </summary>
    /// <remarks>
    /// Cria arquivos em <c>/tmp/proton_<GAMEID></c> com detalhes dos comandos executados.
    /// Útil para troubleshooting avançado ou relatórios de bugs.
    /// Mapeia para a variável de ambiente <c>PROTON_DUMP_DEBUG_COMMANDS=1</c>.
    /// </remarks>
    public bool DumpDebugCommands { get; set; } = false;

    /// <summary>
    /// Ativa a emulação de um cliente Steam para jogos com DRM.
    /// </summary>
    /// <remarks>
    /// Requer configurar <c>STEAM_COMPAT_CLIENT_INSTALL_PATH</c> (não incluído nesta classe).
    /// Útil para jogos da Epic/GOG que esperam o Steam rodando.
    /// Mapeia para a variável de ambiente <c>STEAM_COMPAT_CLIENT_INSTALL_PATH</c>.
    /// </remarks>
    public bool UseSteamCompatClient { get; set; } = false;

    /// <summary>
    /// Ativa logs do Steam Linux Runtime.
    /// </summary>
    /// <remarks>
    /// Gera logs do contêiner Steam Runtime usado pelo UMU.
    /// Útil para depurar problemas no contêiner (ex: falhas de inicialização).
    /// Mapeia para a variável de ambiente <c>STEAM_LINUX_RUNTIME_LOG=1</c>.
    /// </remarks>
    public bool SteamRuntimeLog { get; set; } = false;

    /// <summary>
    /// Ativa o suporte ao Steam Input para controles (Xbox, PS4, PS5, etc.).
    /// </summary>
    /// <remarks>
    /// Habilita mapeamento automático de controles via Steam Input.
    /// Padrão: <c>true</c>, recomendado para jogos com suporte a controles.
    /// Mapeia para a variável de ambiente <c>STEAM_INPUT=1</c>.
    /// </remarks>
    public bool SteamInput { get; set; } = true;

    /// <summary>
    /// Ativa suporte a hidraw para controles avançados (ex: DualSense).
    /// </summary>
    /// <remarks>
    /// Necessário para funcionalidades como toque, giroscópio e hápticos do DualSense.
    /// Padrão: <c>true</c>, ideal para controles modernos.
    /// Mapeia para a variável de ambiente <c>PROTON_ENABLE_HIDRAW=1</c>.
    /// </remarks>
    public bool ProtonEnableHidraw { get; set; } = true;

    /// <summary>
    /// Força o uso do Steam Input para mapeamento de controles.
    /// </summary>
    /// <remarks>
    /// Garante que o Steam Input seja usado, mesmo em jogos sem suporte nativo.
    /// Útil para personalizar botões ou corrigir detecção de controles.
    /// Mapeia para a variável de ambiente <c>PROTON_USE_STEAMINPUT=1</c>.
    /// </remarks>
    public bool ProtonUseSteamInput { get; set; } = false;

    /// <summary>
    /// Ativa pré-calibração para maior precisão de controles.
    /// </summary>
    /// <remarks>
    /// Melhora a precisão de analógicos e outros inputs em jogos sensíveis (ex: FPS).
    /// Mapeia para a variável de ambiente <c>STEAM_INPUT_PRECALIBRATION=1</c>.
    /// </remarks>
    public bool SteamInputPrecalibration { get; set; } = false;

    /// <summary>
    /// Define se a janela do console será oculta durante a execução do <c>umu-run</c>.
    /// </summary>
    /// <remarks>
    /// Quando <c>true</c>, oculta a janela do console para uma experiência de usuário mais limpa.
    /// Configura a propriedade <c>CreateNoWindow</c> do <c>ProcessStartInfo</c>.
    /// Padrão: <c>true</c>.
    /// </remarks>
    public bool HiddenConsole { get; set; } = true;

    /// <summary>
    /// Converte as propriedades em um dicionário de variáveis de ambiente para o UMU/Proton.
    /// </summary>
    /// <returns>Um <c>Dictionary<string, string></c> com as variáveis de ambiente mapeadas.</returns>
    /// <remarks>
    /// Usado para configurar o ambiente do processo <c>umu-run</c>.
    /// Exemplo: <c>{ "WINEPREFIX": "~/rufus-prefix", "PROTON_LOG": "1" }</c>.
    /// Inclui <c>PRESSURE_VESSEL_PREFER_SYSTEM_LIBS=0</c> para evitar conflitos de bibliotecas.
    /// </remarks>
    public Dictionary<string, string> ToDictionary()
        => new()
        {
            { "WINEPREFIX", WinePrefix },
            { "GAMEID", GameId },
            { "STORE", Store },
            { "PROTONPATH", ProtonPath },
            { "PROTON_LOG", ProtonLog ? "1" : "0" },
            { "PROTON_NO_ESYNC", SyncUse == SyncSelect.Async || SyncUse == SyncSelect.Fsync ? "1" : "0" },
            { "PROTON_NO_FSYNC", SyncUse == SyncSelect.Async || SyncUse == SyncSelect.Esync ? "1" : "0" },
            { "PROTON_USE_WINED3D", RenderMode == RenderMode.Wine3d ? "1" : "0" },
            { "PROTON_FORCE_LARGE_ADDRESS_AWARE", ForceLargeAddress ? "1" : "0" },
            { "PROTON_DUMP_DEBUG_COMMANDS", DumpDebugCommands ? "1" : "0" },
            { "STEAM_LINUX_RUNTIME_LOG", SteamRuntimeLog ? "1" : "0" },
            { "STEAM_INPUT", SteamInput ? "1" : "0" },
            { "PROTON_ENABLE_HIDRAW", ProtonEnableHidraw ? "1" : "0" },
            { "PROTON_USE_STEAMINPUT", ProtonUseSteamInput ? "1" : "0" },
            { "STEAM_INPUT_PRECALIBRATION", SteamInputPrecalibration ? "1" : "0" },
            { "PRESSURE_VESSEL_PREFER_SYSTEM_LIBS", "0" }
        };
}

/// <summary>
/// Define o tipo de sincronização para performance multithreading no Proton.
/// </summary>
public enum SyncSelect
{
    /// <summary>
    /// Desativa esync e fsync para máxima compatibilidade. Ideal para jogos problemáticos.
    /// </summary>
    Async,

    /// <summary>
    /// Ativa fsync para alta performance em kernels modernos (ex: Bazzite). Melhora multithreading.
    /// </summary>
    Fsync,

    /// <summary>
    /// Ativa esync para melhor performance em jogos multithreaded. Pode causar crashes em alguns casos.
    /// </summary>
    Esync
}

/// <summary>
/// Define o modo de renderização para gráficos no Proton.
/// </summary>
public enum RenderMode
{
    /// <summary>
    /// Usa WineD3D (OpenGL) para renderização. Ideal para GPUs antigas ou jogos com problemas no Vulkan.
    /// </summary>
    Wine3d,

    /// <summary>
    /// Usa DXVK (Vulkan) para renderização. Recomendado para GPUs modernas devido ao alto desempenho.
    /// </summary>
    Dxvk
}