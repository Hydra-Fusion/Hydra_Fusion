using System;

namespace Hydra.Components.Home;

public static class CustomBreakpointsAdapter
{
    public static Func<double, bool> FaturedMin => length => length > 800;
}