﻿using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("6d091f35-085e-4677-8e93-0d80a3733b95")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://github.com/DKorablin/Plugin.AdbPackageManager")]
#else

[assembly: AssemblyDescription("UI for command tool adb.exe")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2020-2024")]
#endif