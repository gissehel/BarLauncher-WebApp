rm -rf */bin */obj */build build

dotnet.exe publish BarLauncher.WebApp.Wox/BarLauncher.WebApp.Wox.csproj -c Debug
dotnet.exe publish BarLauncher.WebApp.Flow.Launcher/BarLauncher.WebApp.Flow.Launcher.csproj -c Debug -r win-x64

