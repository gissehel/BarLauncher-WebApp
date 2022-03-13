#!/usr/bin/env bash

rm -rf ./*/bin ./*/obj ./build

VERSION=$(cat VERSION)-$(date +%s)

dotnet.exe publish BarLauncher.WebApp.Wox/BarLauncher.WebApp.Wox.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.WebApp.Wox/bin/Debug/net48/publish; zip -r ../../../../../../../BarLauncher-WebApp-${VERSION}.wox .)

dotnet.exe publish BarLauncher.WebApp.Flow.Launcher/BarLauncher.WebApp.Flow.Launcher.csproj -c Debug -p:Version=${VERSION}
(cd build/BarLauncher.WebApp.Flow.Launcher/bin/Debug/net5.0-windows/publish; zip -r ../../../../../../../BarLauncher-WebApp-${VERSION}.zip .)
