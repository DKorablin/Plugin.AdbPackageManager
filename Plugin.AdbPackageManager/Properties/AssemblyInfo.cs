using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("6d091f35-085e-4677-8e93-0d80a3733b95")]
[assembly: System.CLSCompliant(true)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://dkorablin.ru/")]
#else

[assembly: AssemblyTitle("Plugin.AdbPackageManager")]
[assembly: AssemblyDescription("UI for command tool adb.exe")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.AdbPackageManager")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2020-2024")]
#endif