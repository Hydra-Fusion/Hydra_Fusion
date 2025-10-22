using NexusMods.Paths;

namespace Hydra.Infrastructure.Extensions;

public static class FileSystemFactory
{
    public static IFileSystem Create()
    {
        

        throw new PlatformNotSupportedException("Sistema operacional não suportado para FileSystem.");
    }
}