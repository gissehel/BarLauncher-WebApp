rm -rf */bin */obj

dotnet.exe publish BarLauncher.WebApp.Lib/BarLauncher.WebApp.Lib.csproj -c Release -f netstandard2.0
dotnet.exe publish BarLauncher.WebApp.Wox/BarLauncher.WebApp.Wox.csproj -c Release -f net48

