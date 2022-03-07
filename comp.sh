rm -rf */bin */obj

dotnet.exe publish BarLauncher.WebApp.Lib/BarLauncher.WebApp.Lib.csproj -c Release
dotnet.exe publish BarLauncher.WebApp.Wox/BarLauncher.WebApp.Wox.csproj -c Release
dotnet.exe publish BarLauncher.WebApp.Flow.Launcher/BarLauncher.WebApp.Flow.Launcher.csproj -c Release -r win-x64

